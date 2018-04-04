int potPin = A0;    
int potR = 1000;
int val = 0;
float voltage;
float resistance;
float value;

void setup() {
  Serial.begin(9600);
}

void loop() {
  val = analogRead(potPin);                     
  voltage = val * (5.0 / 1024.0);               
  resistance = ((voltage / 5.0) * potR) * 1.5;                          
  Serial.println(resistance);           
  delay(250);                                                    
}

