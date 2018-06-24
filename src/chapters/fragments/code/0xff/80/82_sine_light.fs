// 82 - "Sine Light"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define AM_C 0.41
#define LM_C 1.0


float rings(vec2 p,float offset,float ti){
  float i;
  float t = ti;

  vec3 mv = vec3(sin(t),0, cos(t));
  vec3 pointLight = vec3(1.85 ,mv.z*0.,24.3);

  float ambient = (sin(length(p)*PI*offset + t)+1.)/2.;

  float wavePos = ambient;
  float z = wavePos;
  vec3 point = vec3(p.x, p.y, z*24.);

  vec3 pointToLight = pointLight - point;
  float distToLight = length(pointToLight);
  vec3 pointLightDir = normalize(pointToLight);

  float distance = distToLight;
  distance = distance * distance; 

  float finalAmbient = (ambient * AM_C);

  vec3 tangent = normalize(vec3(1,0,cos(wavePos)));
  vec3 normal = vec3(-tangent.z,0.,tangent.x);//flip
  
  // if(normal.x < pointLightDir.x){discard;}

  float NdotL = max(dot(normal, normalize(pointLightDir)), 1.);
  float finalLambert = NdotL * (LM_C/distance);// * (distToLight* distToLight);

  i = finalLambert +
      finalAmbient;

  return i;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = -u_time;

  i =  rings(p,10. , t*5.);
  i += rings(p,-12. ,t*8.);

  gl_FragColor = vec4(vec3(i),1.);
}