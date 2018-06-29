// 86 - "Preliminary Shadows"

precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define TAU (PI*2.)

const int MaxStep = 400;
const int MaxShadowStep = 500;
const float MaxDist = 1000.;
const float Epsilon = 0.00001;
const float NormEpsilon = 0.001;

mat4 rotateY(float a){
  float c = cos(a);
  float s = sin(a);
  return mat4(c,0,-s,0,0,1,0,0,s,0,c,0,0,0,0,1);
}

/**
 * Signed distance function for a cube centered at the origin
 * with width = height = length = 2.0
 */
float cubeSDF(vec3 p) {
    // If d.x < 0, then -1 < p.x < 1, and same logic 
    // applies to p.y, p.z
    // So if all components of d are negative, then p is inside 
    // the unit cube
    vec3 d = abs(p) - vec3(1.0, 1.0, 1.0);
    
    // Assuming p is inside the cube, how far is it from the surface?
    // Result will be negative or zero.
    float insideDistance = min(max(d.x, max(d.y, d.z)), 0.0);
    
    // Assuming p is outside the cube, how far is it from the surface?
    // Result will be positive or zero.
    float outsideDistance = length(max(d, 0.0));
    
    return insideDistance + outsideDistance;
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
  vec3 mp = mod(p,1.)*2.-1.;

  vec3 np = p;
  np *= .25;
  np.y += 1.3;

  // np = (rotateY(u_time*0.1) * vec4(np,1)).xyz;

  float c = cubeSDF(np);

  float s1 = sdSphere(p+vec3(1,0,0), 1.);
  float s2 = sdSphere(p+vec3(-1,0,0), 1.);

  return min(min(s2,c), s1);
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

  float s = 0.;
  for(int i = 0; i < MaxShadowStep; ++i){
    vec3 v = ro + (rd*s);
    float dist = sdScene(v);

    if(dist < Epsilon){
      return 0.;
    }
    s += dist;

    if(s >= MaxDist){
      return 1.;
    }
  }
  return 1.;
}

float phong(vec3 p, vec3 n, vec3 lightPos){
  vec3 pToLight = vec3(lightPos - p);
  float power = 15.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  
  float nDotL = clamp(dot(n,lightRayDir), 0., 1.);

  float ambient = .1;
  float diffuse = (nDotL*power) / d;
  
  return ambient + diffuse;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i = 0.;
  float t = u_time*1. + 31.;
  vec3 eye = vec3(0, 0, 10.);

  vec3 ray = rayDirection(105.0, u_res, gl_FragCoord.xy);
  vec3 dirLight = normalize(vec3(cos(t*TAU),0,sin(t*TAU) ));
  float ambient = 0.3;
  float s = 10.;
  float d = rayMarch(eye, ray);

  vec3 lightPos = vec3(0, 3, 0);
  lightPos = vec3(cos(t)*s, 1, sin(t)*s);
  
  vec3 point = eye+ray*d;

  if(d < MaxDist){
    float visibleToLight = shadowMarch(eye+ray*(d-0.001), lightPos);
  
    if(visibleToLight == 1.){
      vec3 n = estimateNormal(point);
      
      // i = phong(point, n, vec3(1,12,2));

      i += phong(point, n, lightPos);
    }
    else{
      i = i * visibleToLight + 0.1;
    }
    
    // float intensity = ambient + max(0., dot(dirLight, n));
    // i = visibleToLight;// + intensity;
  }

  gl_FragColor = vec4(vec3(i),1);
}