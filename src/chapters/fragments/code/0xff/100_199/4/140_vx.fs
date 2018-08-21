//
precision highp float;

uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.0001;
const float MaxDist = 400.;
const int MaxSteps = 500;

float sdBox(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz;
  float _in = min(max(d.x, max(d.y, d.z)), 0.0);
  float _out = length(max(d, 0.0));
  return _in + _out;
}

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}

float lighting(vec3 p, vec3 n, vec3 lightPos){
  float ambient = 0.2;

  vec3 pToLight = vec3(lightPos - p);
  float power = 300.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;

  return ambient + diffuse;
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, -z));
}

float sdScene(vec3 p, out float col){
  col = 1.;
  vec3 cellIdx = floor(p);

  vec3 c = vec3(1);
  p = mod(p, c)-(0.125*c);

  float cy = cellIdx.y/4.;

  float sz = c.x/4.;
  float len = length( cellIdx );

  if(len > 15. || len < 14.){
    return sdBox(p, vec3(0.));
  }

  return sdBox(p, vec3(sz) *    (cy* sin( (u_time + cy) *3.)+1.)/2. );
}

vec3 estimateNormal(vec3 v){
  vec3 n;
  float dum;
  n.x = sdScene(vec3(v.x + Epsilon, v.y, v.z),dum) - sdScene( vec3(v.x - Epsilon, v.y, v.z),dum);
  n.y = sdScene(vec3(v.x, v.y + Epsilon, v.z),dum) - sdScene( vec3(v.x, v.y - Epsilon, v.z),dum);
  n.z = sdScene(vec3(v.x, v.y, v.z + Epsilon),dum) - sdScene( vec3(v.x, v.y, v.z - Epsilon),dum);
  return normalize(n);
}

float rayMarch(vec3 ro, vec3 rd, out vec3 col){
  float s = 0.;

  for(int i = 0; i < MaxSteps; i++){
    vec3 p = ro + rd * s;

    float intensity;
    float d = sdScene(p, intensity);
    col = vec3(intensity);

    if(d < Epsilon){
      return s;
    }
    s += d/2.;

    if(d > MaxDist){
      return MaxDist;
    }
  }
  return MaxDist;
}

void main(){
  float dist = 18.;
  float t = u_time/5.;

  vec3 eye = vec3(cos(t)*dist, 18., sin(t)*dist);
  vec3 center = vec3(0);
  vec3 lightPos =  vec3(4) + eye;
  vec3 up = vec3(0,1,0);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 ray = rayDirection(100., u_res, gl_FragCoord.xy);

  vec3 col;
  vec3 worldDir = viewWorld * ray;
  float d = rayMarch(eye, worldDir, col);
  vec3 v = eye + worldDir * d;

  float i;
  if(d < MaxDist){
    vec3 n = estimateNormal(v);
    float lights = lighting(v, n, lightPos);
    i = lights;
  }

  gl_FragColor = vec4(vec3(i), 1);
}