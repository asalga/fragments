// 132
precision highp float;

uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.00001;
const float E = Epsilon;
const float MaxDist = 300.;
const int MaxSteps = 128;
const float PI = 3.141592658;
const float HALF_PI = PI*0.5;
const int MaxShadowStep = 100;

const float START = 0.;

const vec3 lightGrey = vec3(1.);
const vec3 darkGrey = vec3(0.4);

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}


float lighting(vec3 p, vec3 n, vec3 lightPos, vec3 eye){
  float ambient = 0.01;

  // ---
  vec3 pToLight = vec3(eye - p);
  float power = 10.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  float kd = .5;

  float gloss = 300.;
  vec3 V = normalize(eye - p);
  vec3 R = normalize(reflect(-lightPos, n));
  float dotRV = dot(R, V);
  float spec = pow(dotRV, gloss);
  float ks = .4;

  return  ambient +
          kd * diffuse +
          ks * spec;
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, -z));
}

float sdBox(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz;
  float insideDistance = min(max(d.x, max(d.y, d.z)), 0.0);
  float outsideDistance = length(max(d, 0.0));
  return insideDistance + outsideDistance;
}

mat4 r2dY(float a){
  float c = cos(a);
  float s = sin(a);

  return mat4(-c, 0, s,  0,
              0,  1, 0,  0,
              s,  0, c,  0,
              0,  0, 0,  1);
}

mat4 r2dZ(float a){
  float c = cos(a);
  float s = sin(a);

  return mat4(c, -s, 0,  0,
              s,  c, 0,  0,
              0,  0, 1,  0,
              0,  0, 0,  1);
}




float cross2(vec3 p, float s){
  float sc = 1./3.;

  float _one = 100.1;// add a bit extra ;)

  mat4 test = mat4(s,0,0,0,
                    0,s,0,0,
                    0,0,s,0,
                    0,0,0,1);

  float cross =      sdBox( (vec4(p,1.) * test).xyz, vec3(1.));
  cross = min(cross, sdBox( (vec4(p,1.) * test).xyz, vec3(1.)));
  cross = min(cross, sdBox( (vec4(p,1.) * test).xyz, vec3(1.)));

  return cross * (1./s);
}

float cross(vec3 p, float s){
  float sc = (1./3.);

  float _one = 100.;// add a bit extra ;)

  sc = 1.;
  // float cross =
    float a = sdBox(p*1., vec3(_one,sc,sc));
  // cross = min(cross,
    float b = sdBox(p*1., vec3(sc,_one,sc));
  // cross = min(cross,
   float c =  sdBox(p*1., vec3(sc,sc,_one));

  return min(a, min(b,c));
  // return cross * (1./s);
}

// float sdCross( in vec3 p )
// {
//   float da = sdBox(p.xy,vec2(1.0));
//   float db = sdBox(p.yz,vec2(1.0));
//   float dc = sdBox(p.zx,vec2(1.0));
//   return min(da,min(db,dc));
// }

float sdCross( in vec3 p )
{
  float inf = 100.;
  float da = sdBox(p.xyz,vec3(inf,1.0,1.0));
  float db = sdBox(p.yzx,vec3(1.0,inf,1.0));
  float dc = sdBox(p.zxy,vec3(1.0,1.0,inf));
  return min(da,min(db,dc));
}


float sdScene(vec3 p, out float col){

    col = 1.;

   float d = sdBox(p,vec3(1.0));
   // float cube = sdBox(p, vec3(1.0));

   float s = 1.0;
   for( int m=0; m<2; m++ )
   {
      vec3 a = mod( p*s, 2.0 )-1.0;
      s *= 3.0;
      vec3 r = 1.0 - 3.0 * abs(a);

      // float c = sdCross(r)/s;

      float da = max(r.x,r.y);
      float db = max(r.y,r.z);
      float dc = max(r.z,r.x);
      float c = (min(da,min(db,dc))-1.0)/s;

      d = max(d,-c);
      // return cube;
   }
   return d;
   // return vec3(d,0.0,0.0);



  //

  // float cube = sdBox(p, vec3(1.0));
  // float sponge = cube;

  // // float sponge = max(cube, -cross(p, 1.));

  // float c = (1./3.);
  // float s = 2.0;
  // // vec3 np = mod(p, c) - (.5*vec3(c));

  // vec3 np0 = mod(p*s, 2.0) - 1.;

  // // sponge = max(sponge, -cross(p, 1.));
  // sponge = max(sponge, -cross(1. - 3.* abs(np0), 1.));

  // // sponge = max(sponge, -cross(np, 9.));
  // // sponge = cross(np, 3.);

  // // return cross(p*3., 3.)/3.;
  // return sponge;
  // // return max(cube, -cross(p, 1.));
  // // sponge = cross(np , 1.);
  // // return sponge;
  // // return max(cube, -sponge);
  // // return cross(p);
  // // return cross;
  // // return cube;
}


float shadowMarch(vec3 point, vec3 lightPos){

  vec3 pToLight = lightPos-point;
  vec3 rd = normalize(pToLight);
  vec3 ro = point;

  float s = 0.;
  for(int i = 0; i < MaxShadowStep; ++i){
    vec3 v = ro + (rd*s);

    float dum;
    float dist;
    dist = sdScene(v, dum);

    if(dist < Epsilon){
      return 0.;
    }
    s += dist/1.;

    if(s >= MaxDist){
      return 1.;
    }
  }
  return 1.;
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
  float s = START;
  for(int i = 0; i < MaxSteps; i++){
    vec3 p = ro + rd * s;

    float intensity;
    float d = sdScene(p, intensity);
    col = vec3(intensity);

    if(d < Epsilon){
      return s;
    }
    s += d;

    if(d > MaxDist){
      return MaxDist;
    }
  }
  return MaxDist;
}

float ao(vec3 p, vec3 n){
  float stepSize = .02;
  float t = stepSize;
  float oc = 0.;
  float dum;
  for(int i = 0; i < 4; ++i)
  {
    float d = sdScene(p+n*t, dum);
    oc += t - d;
    t += stepSize;
  }

  return clamp(oc, 0., 1.);
}


void main(){
  vec2 fc = gl_FragCoord.xy;
  float t = u_time * .90;
  float i;

  vec3 eye = vec3(cos(t)*5., 5., sin(t)*5.);
  vec3 center = vec3(0.);
  vec3 lightPos = vec3(0.) + eye;
  vec3 up = vec3(0,1,0);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 ray = rayDirection(80.0, u_res, fc);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float d = rayMarch(eye, worldDir, col);

  vec3 v = eye + worldDir*d;

  if(d < MaxDist){
    vec3 n = estimateNormal(v);
    float lights = lighting(v, n, lightPos, eye);
    i += (d * lights) * col.x;
  }

  gl_FragColor = vec4(vec3(i), 1);
}