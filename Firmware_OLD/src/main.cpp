#include <Arduino.h>
#include <WiFi.h>
#include <WebServer.h>
#include <ArduinoJson.h>
// #include <FreeRTOS.h>


const char *SSID = "enigma";
const char *PWD = "kungsg464";
// #define NUM_OF_LEDS 8 
#define PIN 4
 
// Web server running on port 80
WebServer server(80);
 
// JSON data buffer
StaticJsonDocument<250> jsonDocument;
char buffer[250];
 
// env variable
float temperature;
float humidity;
float pressure;

// Forward declation
void handlePost();
void getTest();


void connectToWiFi() {
  Serial.print("Connecting to ");
  Serial.println(SSID);
  
  WiFi.setHostname("TCNCollimation");
  WiFi.begin(SSID, PWD);
  
  while (WiFi.status() != WL_CONNECTED) {
    Serial.print(".");
    delay(500);
    // we can even make the ESP32 to sleep
  }
 
  Serial.print("Connected. IP: ");
  Serial.println(WiFi.localIP());
}
void setup_routing() {	 	 
  server.on("/test", getTest);	 	 
  //server.on("/pressure", getPressure);	 	 
  //server.on("/humidity", getHumidity);	 	 
  //server.on("/env", getEnv);	 	 
  server.on("/led", HTTP_POST, handlePost);	 	 
  	 	 
  // start server	 	 
  server.begin();	 	 
}
 
void create_json(char *tag, float value, char *unit) {  
  jsonDocument.clear();  
  jsonDocument["type"] = tag;
  jsonDocument["value"] = value;
  jsonDocument["unit"] = unit;
  serializeJson(jsonDocument, buffer);
}
 
void add_json_object(char *tag, float value, char *unit) {
  JsonObject obj = jsonDocument.createNestedObject();
  obj["type"] = tag;
  obj["value"] = value;
  obj["unit"] = unit;
}

/*
void read_sensor_data(void * parameter) {
   for (;;) {
     temperature = bme.readTemperature();
     humidity = bme.readHumidity();
     pressure = bme.readPressure() / 100;
     Serial.println("Read sensor data");
 
     // delay the task
     vTaskDelay(60000 / portTICK_PERIOD_MS);
   }
}
*/

void getTest() {
  Serial.println("Get temperature");
  create_json("temperature", temperature, "°C");
  server.send(200, "application/json", buffer);
}

/*
void getHumidity() {
  Serial.println("Get humidity");
  create_json("humidity", humidity, "%");
  server.send(200, "application/json", buffer);
}
 
void getPressure() {
  Serial.println("Get pressure");
  create_json("pressure", pressure, "mBar");
  server.send(200, "application/json", buffer);
}
 
void getEnv() {
  Serial.println("Get env");
  jsonDocument.clear();
  add_json_object("temperature", temperature, "°C");
  add_json_object("humidity", humidity, "%");
  add_json_object("pressure", pressure, "mBar");
  serializeJson(jsonDocument, buffer);
  server.send(200, "application/json", buffer);
}
*/

void handlePost() {
  if (server.hasArg("plain") == false) {
    //handle error here
  }
  String body = server.arg("plain");
  deserializeJson(jsonDocument, body);
  
  // Get RGB components
  int red = jsonDocument["red"];
  int green = jsonDocument["green"];
  int blue = jsonDocument["blue"];
  Serial.print("Red: ");
  Serial.print(red);
  
  // Respond to the client
  server.send(200, "application/json", "{}");
}
/*
void setup_task() {	 	 
  xTaskCreate(	 	 
  read_sensor_data, 	 	 
  "Read sensor data", 	 	 
  1000, 	 	 
  NULL, 	 	 
  1, 	 	 
  NULL 	 	 
  );	 	 
}
*/
void setup() {	 	 
  Serial.begin(9600);	 	 
 	/* 
  // Sensor setup	 	 
  if (!bme.begin(0x76)) {	 	 
    Serial.println("Problem connecting to BME280");	 	 
  }	
  */ 	 
  connectToWiFi();	 	 
  // setup_task();	 	 
  setup_routing(); 	 	 
  // Initialize Neopixel	 	 
  //pixels.begin();	 	 
}	 	 
  	 	 
void loop() {	 	 
  server.handleClient();	 	 
}