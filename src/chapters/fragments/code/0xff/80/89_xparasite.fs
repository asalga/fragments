// 86 - "Lover's Querent"

precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define TAU (PI*2.)

#define OFFSET PI/4.

const int MaxStep = 370;
const int MaxShadowStep = 200;
const float MaxDist = 1000.;
const float Epsilon = 0.00001;
const float NormEpsilon = 0.0001;

mat4 rotateY(float a){
  float c = cos(a);
  float s = sin(a);
  return mat4(c,0,-s,0,0,1,0,0,s,0,c,0,0,0,0,1);
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
    vec2 xy = fragCoord - size / 2.0;
    float z = size.y / tan(radians(fieldOfView) / 2.0);
    return normalize(vec3(xy, -z));
}

float sdSphere(vec3 p, float r){
  return length(p)-r;
}

float sdScene(vec3 p){
  float t = u_time*4.;

  p = (vec4(p,1) * rotateY(t/10.)).xyz;

  float dy =  sin(7.0 * p.y + t ) * .08;
  float dx =  cos(5.0 * p.x + -t) * 1.5;
  float dz =  cos(3.0 * p.z + t+3.)  * 1.8;

  return (dy * dx * dz)/1. + sdSphere(p, 1.);
}

vec3 estimateNormal(vec3 p) {
    return normalize(vec3(
        sdScene(vec3(p.x + Epsilon, p.y, p.z)) - sdScene(vec3(p.x - Epsilon, p.y, p.z)),
        sdScene(vec3(p.x, p.y + Epsilon, p.z)) - sdScene(vec3(p.x, p.y - Epsilon, p.z)),
        sdScene(vec3(p.x, p.y, p.z  + Epsilon)) - sdScene(vec3(p.x, p.y, p.z - Epsilon))
    ));
}

float rayMarch(vec3 ro, vec3 rd){
  float s = 0.;
  for(int i = 0; i < MaxStep; ++i){
    vec3 v = ro + (rd*s);
    float dist = sdScene(v);

    if(dist < Epsilon){
      return s;
    }
    s += dist;

    if(s >= MaxDist){
      return MaxDist;
    }
  }
  return MaxDist;
}



float shadowMarch(vec3 point, vec3 lightPos){

  vec3 pToLight = lightPos-point;
  vec3 rd = normalize(pToLight);
  vec3 ro = point;

  float t = 0.;
  float res = 1.;
  float k = 120.;

  for(int i = 0; i < MaxShadowStep; ++i){
    float h = sdScene(ro + (rd*t));

    if(h < Epsilon){
      return 0.;
    }

    res = min(res, k * h/(float(i)*2.) );
    t += h;

    // if(t >= MaxDist){
      // return res;
    // }
  }

  return res;
}

float phong(vec3 p, vec3 n, vec3 lightPos){
  vec3 pToLight = vec3(lightPos - p);
  float power = 16.;
  float gloss = 500.;

  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  
  float nDotL = max(dot(n,lightRayDir), 0.);



  vec3 H = normalize( pToLight  );
  float NdotH = dot(n, H);


  float ambient = 0.1;
  float spec = pow( clamp(NdotH, 0., 1.) , gloss );
  float diffuse = (nDotL*power) / d;
  
  return ambient + diffuse + spec;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i = 0.;
  float t = u_time*1. + 31.;
  vec3 eye = vec3(0, 0, 2.6);

  vec3 ray = rayDirection(105.0, u_res, gl_FragCoord.xy);
  vec3 dirLight = normalize(vec3(cos(t*TAU),0,sin(t*TAU) ));
  float ambient = 0.3;
  float s = 10.;
  float d = rayMarch(eye, ray);

  vec3 lightPos = vec3(0, 5., 6. + sin(t*10.)*.3);
  // lightPos = vec3(cos(t)*s, 1, sin(t)*s);
  
  vec3 point = eye+ray*d;

  if(d < MaxDist){
    float visibleToLight = shadowMarch(eye+ray*(d-0.0001), lightPos);  
    vec3 n = estimateNormal(point);
    float l = phong(point, n, lightPos);
    i = l + visibleToLight + ambient;
  }

  gl_FragColor = vec4(vec3(i),1);
}