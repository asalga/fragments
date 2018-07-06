// 91 - rings
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define TAU (PI*2.)

const int MaxStep = 170;
const int MaxShadowStep = 100;
const float MaxDist = 1000.;
const float Epsilon = 0.0001;

mat4 rotateY(float a){
  float c = cos(a);
  float s = sin(a);
  return mat4(c, 0,-s, 0,
              0, 1, 0, 0,
              s, 0, c, 0,
              0, 0, 0, 1);
}

mat4 rotateZ(float a){
  float c = cos(a);
  float s = sin(a);
  return mat4(c,-s, 0, 0,
              s, c, 0, 0,
              0, 0, 1, 0,
              0, 0, 0, 1);
}

float sdTorus( vec3 p, vec2 t){
  vec2 q = vec2(length(p.xz)-t.x,p.y);
  return length(q)-t.y;
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.;
  float z = size.y/tan(radians(fieldOfView)/2.);
  return normalize(vec3(xy, -z));
}

float sdSphere(vec3 p, float r){
  return length(p)-r;
}

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}

vec3 r2dZ(vec3 p, float r){
  return (vec4(p,1) * rotateZ(r)).xyz;
}

float m(float t, float md, float s){
  float ti = t;
  return step( mod(t, md) , s) * (1./s)*2. * PI * smoothstep(0.,1.,fract(ti));
}

float sdScene(vec3 p){
  float w = 0.2;
  float s = 5.0;
  float t = u_time*.1;
  float div = 1./5.;
  
  float c0 = sdSphere(p, 0.5);  
  float i = c0;

  for(int it = 0;it < 5;++it){
    float _s1 = m(t - div * float(it), 1., div);
    float sz = 0.2 + float(it)/10.;
    float s1 = sdTorus(r2dZ(p, _s1), vec2(sz * s, w));
    i = min(i, s1);
  }
  
  // float s2 = sdTorus(r2dZ(p, _s2), vec2(.3 * s, w));
  // float s3 = sdTorus(r2dZ(p, _s3), vec2(.4 * s, w));
  // float s4 = sdTorus(r2dZ(p, _s4), vec2(.5 * s, w));
  // float s5 = sdTorus(r2dZ(p, _s5), vec2(.6 * s, w));

  return i;
}


float ao(vec3 p, vec3 n){
  float stepSize = .01;
  float t = stepSize;
  float oc = 0.;
  for(int i = 0; i < 10; ++i)
  {
    float d = sdScene(p+n * t);
    oc += t - d;
    t += stepSize;
  }
  return clamp(oc, 0., 1.);
}

vec3 estimateNormal(vec3 v){
  vec3 n;
  n.x = sdScene(vec3(v.x + Epsilon, v.y, v.z)) - sdScene( vec3(v.x - Epsilon, v.y, v.z));
  n.y = sdScene(vec3(v.x, v.y + Epsilon, v.z)) - sdScene( vec3(v.x, v.y - Epsilon, v.z));
  n.z = sdScene(vec3(v.x, v.y, v.z + Epsilon)) - sdScene( vec3(v.x, v.y, v.z - Epsilon));
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
  
  float nDotL = max(dot(n,lightRayDir), 0.);

  float ambient = .15;
  float diffuse = (nDotL*power) / d;
  
  return ambient + diffuse;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i = 0.;
  float t = u_time;
  vec3 ray = rayDirection(100.0, u_res, gl_FragCoord.xy);


  vec3 center = vec3(0,0,0);
  vec3 up = vec3(0,1,0);
  vec3 eye = vec3(-4.5, 4, 2);
  vec3 center = eye + vec3(0,0,-1);
  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 worldDir = viewWorld * ray;


  float d = rayMarch(eye, worldDir);
  vec3 lightPos = vec3(1, 4, 2);
  
  vec3 point = eye+worldDir*d;  

  float ambient = 0.;

  if(d < MaxDist){
    float visibleToLight = shadowMarch(eye+worldDir*(d-0.001), lightPos);
    vec3 n = estimateNormal(point);
    float lambert = phong(point, n, lightPos);
    float ao = ao(point, n);

    if(visibleToLight == 1.){
      i += lambert * 1.-(ao*1.0);
    }
    else {
      i = 0.1; 
    }
  }

  gl_FragColor = vec4(vec3(i),1);
}