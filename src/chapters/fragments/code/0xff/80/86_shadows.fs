precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define TAU (PI*2.)

const int MaxStep = 200;
const float MaxDist = 2000.;
const float Epsilon = 0.001;
const float NormEpsilon = 0.00001;

mat4 rotateY(float a){
  float c = cos(a);
  float s = sin(a);

  return mat4(c,0,-s,0,
              0,1,0,0,
              s,0,c, 0,
              0,0,0,1);
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
  // vec3 mp = mod(p,1.)*2.-1.;

  vec3 np = p;
  np *= .25;
  np.y += 1.5;

  // np = (rotateY(u_time*0.5) * vec4(np,1)).xyz;

  float c = cubeSDF(np);

  float s1 = sdSphere(p+vec3(0,3,0), 2.);
  float s2 = sdSphere(p, 1.);

  // return c;
  return min(s2,c);
}

vec3 estimateNormal(vec3 p){
  vec3 n;
  n.x = sdScene( vec3(p.x + NormEpsilon, p.y, p.z) - 
    vec3(p.x - NormEpsilon, p.y, p.z));
  n.y = sdScene( vec3(p.x, p.y + NormEpsilon, p.z) - 
    vec3(p.x, p.y - NormEpsilon, p.z));
  n.z = sdScene( vec3(p.x, p.y, p.z + NormEpsilon) - 
    vec3(p.x, p.y, p.z - NormEpsilon));
  return normalize(n);
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



float ss(vec3 ro, vec3 rd){
  float s = 0.;

  for(int i = 0; i < 6; ++i){
    vec3 v = ro + (rd*s);
    float dist = sdScene(v);

    if(dist < Epsilon){
      return 0.;
    }
    s += dist;
  }
  return 1.;
}

float rayMarchShadow(vec3 point, vec3 lightPos){
  vec3 pToLight = lightPos-point;
  vec3 rd = normalize(pToLight);
  float d = ss(point, rd);
  
  // if distance we got is basically distance to light
  return d;
  // if( d < length(pToLight)){
    // return 1.;
  // }

  // float d = 0.;
  // for(int i = 0; i < maxt; ++i){
  //   d = samplePoint + 
  //   // if we hit something, return 
  //   if(d < Epsilon){
  //     return 0.;
  //   }
  // }

  // return 0.;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i = 0.;
  float t = u_time*5.;

  vec3 eye = vec3(0, 0, 3.);

  //eye.z  += (sin(t*.25)+1.)/2.

  vec3 ray = rayDirection(130.0, u_res, gl_FragCoord.xy);
  vec3 dirLight = normalize(vec3(cos(t*TAU),0,sin(t*TAU) ));
  float ambient = 0.3;
  float s = 5.;
  float d = rayMarch(eye, ray);

  vec3 lightPos = vec3(0, 10, 1);
    lightPos = vec3(cos(t)*s, 4, sin(t)*s);
  

  // vec3 n = estimateNormal(eye+ray*d);
  if(d < MaxDist){
    float visibleToLight = rayMarchShadow(eye+ray*d/1.001, lightPos);

    // float intensity = ambient + max(0., dot(dirLight, n));
    // i = intensity;
    i = visibleToLight;// + intensity;
  }

  gl_FragColor = vec4(vec3(i),1);
}