typedef struct { 
  uint8_t pin;
  char* device_type;
  char* serial_number;
  bool enable;
  char* switch_topic;
}ledObject;

typedef struct { 
  char* device_type;
  char* serial_number;
}temperatureSensor;


