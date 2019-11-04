// Using ESP8266 microcontroller NodeMCU Lua WiFi with CP2102 USB

#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <OneWire.h>
#include <DallasTemperature.h>
#include "device_struct.h"

#define PIN_D0 16
#define PIN_D1 5
#define PIN_D2 4
#define PIN_D3 0
#define PIN_D4 2
#define PIN_D5 14
#define PIN_D6 12
#define PIN_D7 13
#define PIN_D8 15

#define MAC WiFi.macAddress()
#define LED_DEVICE_COUNT 2
#define TEMP_DEVICE_COUNT 2
#define TEMPERATURE_PRECISION 9
#define BUFFER_SIZE 100

#define MQTT_HELLO_TOPIC (String("devices/6519515/hello").c_str())
#define MQTT_LED_STATUS_TOPIC (String("devices/6519515/led/status").c_str())
#define MQTT_LED_STATUS_RESPONSE_TOPIC (String("devices/6519515/led/status/response").c_str())
#define MQTT_LED_SWITCH_RESPONSE_TOPIC (String("devices/6519515/led/switch/response").c_str())
#define MQTT_ERROR_TOPIC (String("devices/6519515/errors").c_str())
#define MQTT_TEMP_RESPONSE_TOPIC (String("devices/6519515/temperature/data").c_str())

const char *chip_id = "6519515";

const char *ssid =  "<SSID_NAME>";  // Name of WiFi endpoint
const char *pass =  "<SSID_PASSWORD>"; // WiFi password

const char *mqtt_server = "<MQTT_SERVER_NAME>"; // MQTT server name
const int mqtt_port = 1883; // MQTT connection port
const char *mqtt_user = "<MQTT_USERNAME>"; // MQTT username
const char *mqtt_pass = "<MQTT_PASSWORD>"; // MQTT password

// Create OneWire & DallasTemperature for each temperature sensor
// Multiple (1 pin = multiple devices)
//OneWire oneWire(PIN_D4);
//DallasTemperature DS18B20(&oneWire);
//DeviceAddress addresses[TEMP_DEVICE_COUNT];

// OR (1 pin = 1 device)
OneWire oneWire[] { PIN_D4, PIN_D3 };
DallasTemperature sensors[TEMP_DEVICE_COUNT];
DeviceAddress address;

WiFiClient wclient;
PubSubClient client(wclient, mqtt_server, mqtt_port);

// pin---type---serial number---enable---switch topic
ledObject ledObjectArr[LED_DEVICE_COUNT] {
  {PIN_D1, "led", "MY7LY28PL6593R56", false, "devices/6519515/led/MY7LY28PL6593R56/switch"},
  {PIN_D7, "led", "VF7HC363C2895R47", false, "devices/6519515/led/VF7HC363C2895R47/switch"}
};

// type---serial number
temperatureSensor temperatureSensorArr[TEMP_DEVICE_COUNT] {
  {"temperature", "GX98S76M6QZ23U32"},
  {"temperature", "6G5VF42Z45927573"}
};

int tm = 3000;
bool hello_message = false;

void setup() {

  Serial.begin(115200);
  delay(10);
  ESP8266_INFO();

  //////////////////////////// 1 PIN = MULTIPLE SENSORS //////////////////////////
  /*DS18B20.begin();

  // locate devices on the bus
  Serial.print("Locating temperature devices...");
  Serial.print("Found ");
  Serial.print(DS18B20.getDeviceCount(), DEC);
  Serial.println(" devices.");

  // report parasite power requirements
  Serial.print("Parasite power is: ");
  if (DS18B20.isParasitePowerMode()) Serial.println("ON");
  else Serial.println("OFF");

  oneWire.reset_search();
  for (uint8_t i = 0; i < TEMP_DEVICE_COUNT; ++i) {
  if (!oneWire.search(addresses[i])) {
    Serial.print("Unable to find address for Thermometer № ");
    Serial.println(i);
  }
  else {
      // show the addresses we found on the bus
      Serial.printf("Device %d Address: ", i);
      printAddress(addresses[i]);
      Serial.println();
  }
  DS18B20.setResolution(addresses[i], TEMPERATURE_PRECISION)

  Serial.printf("Device %d Resolution: ", i);
    Serial.print(DS18B20.getResolution(addresses[i]), DEC);
    Serial.println();
  }*/
  ////////////////////////////////////////////////////////////////////////////////

  //////////////////////////// 1 PIN = 1 SENSOR //////////////////////////////////
  for (uint8_t i = 0; i < TEMP_DEVICE_COUNT; ++i) {
    sensors[i].setOneWire(&oneWire[i]);
    sensors[i].begin();

    Serial.printf("Sensor %d\n", i);
    Serial.print("Locating temperature devices...");
    Serial.print(" Found ");
    Serial.print(sensors[i].getDeviceCount(), DEC);
    Serial.println(" devices.\n");

    Serial.print("Parasite power is: ");
    if (sensors[i].isParasitePowerMode()) Serial.println("ON");
    else Serial.println("OFF");

    if (!sensors[i].getAddress(address, 0)) {
      Serial.print("Unable to find address for Thermometer ");
      Serial.println(i);
    }
    else {
      Serial.printf("Device %d Address: ", i);
      printAddress(address);
      Serial.println();
    }

    sensors[i].setResolution(address, TEMPERATURE_PRECISION);

    Serial.printf("Device %d Resolution: ", i);
    Serial.print(sensors[i].getResolution(address), DEC);
    Serial.println();
  }
  ////////////////////////////////////////////////////////////////////////////////

  Serial.println();

  pinMode(PIN_D1, OUTPUT);
  pinMode(PIN_D7, OUTPUT);
}

/*-----------------------------------------------------------------------------------------------
 * Function: printAddress
 * Description: Function to print a device address
 * ----------------------------------------------------------------------------------------------*/
void printAddress(DeviceAddress deviceAddress)
{
  for (uint8_t i = 0; i < 8; i++)
  {
    // zero pad the address if necessary
    if (deviceAddress[i] < 16) Serial.print("0");
    Serial.print(deviceAddress[i], HEX);
  }
}

/*-----------------------------------------------------------------------------------------------
 * Function: ESP8266_INFO
 * Description: Get general info about ESP8266 controller
 * ----------------------------------------------------------------------------------------------*/
void ESP8266_INFO()
{
  //reference:
  // http://esp8266.github.io/Arduino/versions/2.1.0/doc/libraries.html
  // http://arduino-er.blogspot.tw/2016/04/nodemcuesp8266-get-esp-chip-and-flash.html
  // https://techtutorialsx.com/2017/04/09/esp8266-get-mac-address/

  Serial.println();
  Serial.println("ESP8266 Info.");
  Serial.println("****************************************");

  Serial.print("WiFi MAC: ");
  Serial.println(MAC);
  Serial.printf("Chip ID as a 32-bit integer:\t%08X\n", ESP.getChipId());
  Serial.printf("Flash chip ID as a 32-bit integer:\t\t%08X\n", ESP.getFlashChipId());
  Serial.printf("Flash chip frequency:\t\t\t\t%d (Hz)\n", ESP.getFlashChipSpeed());
  Serial.printf("Flash chip size:\t\t\t\t%d (bytes)\n", ESP.getFlashChipSize());
  Serial.printf("Free heap size:\t\t\t\t\t%d (bytes)\n", ESP.getFreeHeap());

  Serial.println("****************************************");
  Serial.println();
}

/*-----------------------------------------------------------------------------------------------
 * Function: callback
 * Description: Function for receiving and processing messages from MQTT broker
 * ----------------------------------------------------------------------------------------------*/
void callback(const MQTT::Publish& pub)
{
  Serial.print(pub.topic());
  Serial.print(" => ");
  Serial.print(pub.payload_string());
  Serial.println();

  String payload = pub.payload_string();
  String topic = pub.topic();
  //char *topic_char = const_cast<char*>(topic.c_str());
  //char *payload_char = const_cast<char*>(payload.c_str());

  ledObject *led;

  if (pub.topic() == MQTT_LED_STATUS_TOPIC) {
    // If we need to get the status of all led
    if (payload == "") {
      for (uint8_t i = 0; i < LED_DEVICE_COUNT; ++i) {
        String response = String("{ \n\t\"type\": \"led\", \n\t\"enable\": \"") + String(ledObjectArr[i].enable) + "\", \n\t\"serial_number\": \"" + ledObjectArr[i].serial_number + "\" \n}";
        Serial.printf("Response:\n %s\n", response.c_str());
        client.publish(MQTT_LED_STATUS_RESPONSE_TOPIC, response);
      }
      return;
    }

    // Find LED status by serial number
    led = FIND_LED_BY_SN(payload);
    if (String(led->serial_number) != payload) {
      Serial.printf("LED with serial number %s was not found\n", payload.c_str());
      return;
    }
    String response = String("{ \n\t\"type\": \"led\", \n\t\"enable\": \"") + String(led->enable) + "\", \n\t\"serial_number\": \"" + led->serial_number + "\" \n}";
    Serial.printf("Response: \n%s\n\n", response.c_str());
    client.publish(MQTT_LED_STATUS_RESPONSE_TOPIC, response);
    return;
  }

  // Otherwise it is a request to enable/disable led
  led = FIND_LED_BY_TOPIC(topic);
  if (String(led->switch_topic) == topic) {
    int led_state = payload.toInt();
    if (led_state != led->enable) {
      digitalWrite(led->pin, !led->enable);
      led->enable = !led->enable;

      // Send result response
      String response = String("{ \n\t\"type\": \"led\", \n\t\"serial_number\": \"") + String(led->serial_number) + "\", \n\t\"result\": \"OK\" \n}";
      client.publish(MQTT_LED_SWITCH_RESPONSE_TOPIC, response);
      return;
    }
    String response = String("{ \n\t\"type\": \"led\", \n\t\"serial_number\": \"") + String(led->serial_number) + "\", \n\t\"result\": \"NO_CHANGE\" \n}";
    client.publish(MQTT_LED_SWITCH_RESPONSE_TOPIC, response);
  }
}

void loop() {

  if (!WiFi_CONNECT())
    return;

  if (!MQTT_CONNECT())
    return;

  client.loop();

  if (tm == 0) {
    // call sensors.requestTemperatures() to issue a global temperature
    // request to all devices on the bus

    //////////////////////////// 1 PIN = MULTIPLE SENSORS //////////////////////////
    /*Serial.print("Requesting temperatures...");
    DS18B20.requestTemperatures();
    Serial.println("DONE");

    for (uint8_t i = 0; i < TEMP_DEVICE_COUNT; ++i) {
      if (temperatureSensorArr[i].device_type == "temperature") {
        DS18B20_CELSIUS(addresses[i], temperatureSensorArr[i].serial_number);
      }
    }*/
    ////////////////////////////////////////////////////////////////////////////////

    //////////////////////////// 1 PIN = 1 SENSOR //////////////////////////////////
    for (uint8_t i = 0; i < TEMP_DEVICE_COUNT; ++i) {
      if (temperatureSensorArr[i].device_type == "temperature") {
        Serial.print("Sensor ");
        Serial.print(1);
        Serial.print(" requesting temperatures...");
        sensors[i].requestTemperatures();
        Serial.println("DONE");

        DS18B20_CELSIUS_1(sensors[i], address, temperatureSensorArr[i].serial_number);
      }
    }
    ////////////////////////////////////////////////////////////////////////////////
    tm = 3000;  // pause around 30 seconds
  }
  tm--;

  delay(10);
}


/*-----------------------------------------------------------------------------------------------
 * Function: WiFi_CONNECT
 * Description: Connection to the specified wifi point using ssid & ssid_pass
 * Outs: True or false connect result
 * ----------------------------------------------------------------------------------------------*/
bool WiFi_CONNECT()
{
  if (WiFi.status() != WL_CONNECTED) {
    Serial.print("Connecting to ");
    Serial.print(ssid);
    Serial.println("...");
    WiFi.begin(ssid, pass);

    if (WiFi.waitForConnectResult() != WL_CONNECTED)
      return false;
    Serial.println("WiFi connected");
  }
  return true;
}

/*-----------------------------------------------------------------------------------------------
 * Function: MQTT_CONNECT
 * Description: Connection to the specified MQTT broker using mqtt_user & mqtt_pass
 * Outs: True or false connect result
 * ----------------------------------------------------------------------------------------------*/
bool MQTT_CONNECT()
{
  if (WiFi.status() == WL_CONNECTED) {
    if (!client.connected()) {
      Serial.printf("Connecting to MQTT server %s...\n", mqtt_server);

      if (client.connect(MQTT::Connect("6519515").set_auth(mqtt_user, mqtt_pass))) {
        Serial.println("Connected to MQTT server");
        SEND_HELLO_MESSAGE();
        client.set_callback(callback);

        // subscribe to topics with data for LEDs
        for (uint8_t i = 0; i < LED_DEVICE_COUNT; ++i) {
          if (ledObjectArr[i].device_type == "led") {
            //String topic = String("devices/") + CHIP_ID + "/" + ledObjectArr[i].serial_number + "/" + ledObjectArr[i].device_type;
            client.subscribe(ledObjectArr[i].switch_topic);
            Serial.printf("Subscribing to topic: %s \n", ledObjectArr[i].switch_topic);
          }
        }
        //String status_topic = MQTT_LED_STATUS_TOPIC + String("/") + ledObjectArr[i].serial_number + "/" + ledObjectArr[i].device_type;
        client.subscribe(MQTT_LED_STATUS_TOPIC);
        Serial.printf("Subscribing to topic: %s \n", MQTT_LED_STATUS_TOPIC);

        return true;
      } else {
        Serial.println("Could not connect to MQTT server");
        return false;
      }
    }
    return true;
  }
  return false;
}

/*-----------------------------------------------------------------------------------------------
 * Function: FIND_LED_BY_SN
 * Description: Searches for the serial number in the declared array of LED objects
 * Ins: Serial number of led
 * Outs: Return ledObject if exists / NULL if not exists
 * ----------------------------------------------------------------------------------------------*/
ledObject *FIND_LED_BY_SN(String serial_number)
{
  Serial.println("\n\nDEBUG: FIND_LED_BY_SN function execution");
  ledObject tmp;
  for (uint8_t i = 0; i < LED_DEVICE_COUNT; ++i) {
    if (String(ledObjectArr[i].serial_number) == serial_number) {
      Serial.println("DEBUG: LED FOUND!\n\n");
      return &ledObjectArr[i];
    }
  }
  Serial.println("DEBUG: LED NOT FOUND\n\n");
  return &tmp;
}

/*-----------------------------------------------------------------------------------------------
 * Function: FIND_LED_BY_TOPIC
 * Description: Searches for the switch topic in the declared array of LED objects
 * Ins: Serial number of led
 * Outs: Return ledObject if exists / NULL if not exists
 * ----------------------------------------------------------------------------------------------*/
ledObject *FIND_LED_BY_TOPIC(String switch_topic)
{
  Serial.println("\n\nDEBUG: FIND_LED_BY_TOPIC function execution");
  ledObject tmp;
  for (uint8_t i = 0; i < LED_DEVICE_COUNT; i++) {
    if (String(ledObjectArr[i].switch_topic) == switch_topic) {
      Serial.println("DEBUG: LED FOUND!\n\n");
      return &ledObjectArr[i];
    }
  }
  Serial.println("DEBUG: LED NOT FOUND\n\n");
  return &tmp;
}

/*-----------------------------------------------------------------------------------------------
 * Function: FIND_addresses_BY_SN
 * Description: Searches for the serial number in the declared array of temperature sensors
 * Ins: Serial number of temperature sensor
 * Outs: Return temperatureSensor if exists / NULL if not exists
 * ----------------------------------------------------------------------------------------------*/
temperatureSensor *FIND_addresses_BY_SN(String serial_number)
{
  Serial.println("\n\nDEBUG: FIND_addresses_BY_SN function execution");
  //Serial.printf("DEBUG: intup: serial_number - %s", serial_number);
  temperatureSensor tmp;
  for (uint8_t i = 0; i < TEMP_DEVICE_COUNT; ++i) {
    if (String(temperatureSensorArr[i].serial_number) == serial_number) {
      Serial.println("DEBUG: SENSOR FOUND!\n\n");
      return &temperatureSensorArr[i];
    }
  }
  Serial.println("DEBUG: SENSOR NOT FOUND\n\n");
  return &tmp;
}

/*-----------------------------------------------------------------------------------------------
 * Function: SEND_HELLO_MESSAGE
 * Description: When starting the device sends a welcome message for each sensor/led to recognize
 * ----------------------------------------------------------------------------------------------*/
void SEND_HELLO_MESSAGE()
{
  if (!hello_message) {
    // LED
    for (uint8_t i = 0; i < LED_DEVICE_COUNT; ++i) {
      String message = String("{\n\t\"type\": \"") + ledObjectArr[i].device_type + "\", \n\t\"serial_number\": \"" + ledObjectArr[i].serial_number + "\"\n}";
      Serial.printf("Send hello message:%s \n", message.c_str());
      client.publish(MQTT_HELLO_TOPIC, message);
    }
    // TEMPERATURE
    for (uint8_t i = 0; i < TEMP_DEVICE_COUNT; ++i) {
      String message = String("{ \n\t\"type\": \"") + temperatureSensorArr[i].device_type + "\", \n\t\"serial_number\": \"" + temperatureSensorArr[i].serial_number + "\"\n}";
      Serial.printf("Send hello message:%s \n", message.c_str());
      client.publish(MQTT_HELLO_TOPIC, message);
    }
    hello_message = !hello_message;
  }
}

/*-----------------------------------------------------------------------------------------------
 * Function: DS18B20_CELSIUS
 * Description: Get celsius reading from DS18B20 Temperature sensor
 * Ins: Integer value for sensor address
 * Outs: Publish to MQTT broker celsius reading
 * ----------------------------------------------------------------------------------------------*/
/*void DS18B20_CELSIUS(DeviceAddress deviceAddress, char* serial_number)
{
  float tempC = DS18B20.getTempC(deviceAddress);
  if (tempC == DEVICE_DISCONNECTED_C)
  {
    Serial.printf("Temperature sensor disconnected: serial number - %s\n", serial_number);
    String message = String("{ \n\t\"type\": \"temperature\", \n\t\"message\": \"disconnected\", \n\t\"serial_number\": \"") + serial_number + "\"\n}";
    client.publish(MQTT_ERROR_TOPIC, message);
    return;
  }

  String data = String("{ \n\t\"type\": \"temperature\", \n\t\"serial_number\": \"") + serial_number + "\", \n\t\"data\": \"" + String(tempC) + "\"\n}";
  client.publish(MQTT_TEMP_RESPONSE_TOPIC, data);

  Serial.print(serial_number);
  Serial.print(" Temp C: ");
  Serial.print(tempC);
  Serial.print(" Temp F: ");
  Serial.print(DallasTemperature::toFahrenheit(tempC));
}*/

/*-----------------------------------------------------------------------------------------------
 * Function: DS18B20_CELSIUS_1
 * Description: Get celsius reading from DS18B20 Temperature sensor
 * Ins: Integer value for sensor address
 * Outs: Publish to MQTT broker celsius reading
 * ----------------------------------------------------------------------------------------------*/
void DS18B20_CELSIUS_1(DallasTemperature sensor, DeviceAddress deviceAddress, char* serial_number)
{
  float tempC = sensor.getTempC(deviceAddress);
  if (tempC == DEVICE_DISCONNECTED_C)
  {
    Serial.printf("Temperature sensor disconnected: serial number - %s\n", serial_number);
    String message = String("{ \n\t\"type\": \"temperature\", \n\t\"message\": \"disconnected\", \n\t\"serial_number\": \"") + serial_number + "\"\n}";
    client.publish(MQTT_ERROR_TOPIC, message);
    return;
  }

  String data = String("{ \n\t\"type\": \"temperature\", \n\t\"serial_number\": \"") + serial_number + "\", \n\t\"temperature_data\": \"" + String(tempC) + "\"\n}";
  client.publish(MQTT_TEMP_RESPONSE_TOPIC, data);

  Serial.print(serial_number);
  Serial.print(" Temp C: ");
  Serial.print(tempC);
  Serial.print(" Temp F: ");
  Serial.print(DallasTemperature::toFahrenheit(tempC));
  Serial.println();
}

