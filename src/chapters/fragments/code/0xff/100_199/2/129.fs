// 129 - "Invade Space"
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.01;
const float MaxDist = 400.;
const int MaxSteps = 128;
const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI*0.5;

float sdBox(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz;
  float insideDistance = min(max(d.x, max(d.y, d.z)), 0.);
  float outsideDistance = length(max(d, 0.));
  return insideDistance + outsideDistance;
}

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}

float lighting(vec3 p, vec3 n, vec3 lightPos, vec3 eye){
  float ambient = 0.35;

  // ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 20.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  float kd = 0.8;

  float gloss = 15.;
  vec3 V = normalize(eye - p);
  vec3 R = normalize(reflect(-lightPos, n));
  float dotRV = dot(R, V);
  float spec = pow(dotRV, gloss);
  float ks = 0.05;

  return  ambient +
          kd * diffuse +
          ks * spec;
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, -z));
}

mat4 r2dY(float a){
  float c = cos(a);
  float s = sin(a);

  return mat4(-c, 0, s,  0,
              0,  1, 0,  0,
              s,  0, c,  0,
              0,  0, 0,  1);
}
mat4 r2dX(float a){
  float c = cos(a);
  float s = sin(a);

  return mat4(1,  0,  0,  0,
              0,  -c, s,  0,
              0,  s,  c,  0,
              0,  0,  0,  1);
}
mat4 r2dZ(float a){
  float c = cos(a);
  float s = sin(a);

  return mat4(c, -s, 0,  0,
              s,  c, 0,  0,
              0,  0, 1,  0,
              0,  0, 0,  1);
}
mat4 scale(float x, float y, float z){
  return mat4(x, 0, 0, 0,
              0, y, 0, 0,
              0, 0, z, 0,
              0, 0, 0, 1);
}

float halfInvader(vec3 p, float state){

  vec3 c = vec3(14, 10, 0);
  p = mod(p, c)-0.5*c;

  float px = 1.0;
  float sz = 0.5;

  // from center to left
  float _0 = sdBox(p,                        vec3(sz,sz*4.,sz));
  float _1 = sdBox(p+vec3(px*1.,0,0),        vec3(sz,sz*4.,sz));
  float _2 = sdBox(p+vec3(px*2.,-px/2.,0),   vec3(sz,sz*5.,sz));
  float _3 = sdBox(p+vec3(px*3.,px/2.,0),    vec3(sz,sz*5.,sz));
  float _4 = sdBox(p+vec3(px*4.,0,0),        vec3(sz,sz*2.,sz));
  float _5 = sdBox(p+vec3(px*3.,-px*3.5,0),  vec3(sz,sz,sz));//antena
  float eye = sdBox(p + vec3(px*2.,-px*.5,0), vec3(sz,sz,sz*1.1));
  float legs;
  float arms;

  if(state == 0.){//arms down
   arms = sdBox(p+vec3(px*5.,px*1.5,0),   vec3(sz,sz*3.,sz));
   legs = sdBox(p+vec3(px*1.5,px*3.5,0),  vec3(sz*2.,sz,sz));
  }
  else {
    arms = sdBox(p+vec3(px*5.,-px*1.,0),   vec3(sz,sz*4.,sz));
    legs = sdBox(p+vec3(px*4.,px*3.5,0),  vec3(sz,sz,sz));
  }

  float final = _0;
  final = min(final,_1);
  final = min(final,_2);
  final = min(final,_3);
  final = min(final,_4);
  final = min(final,_5);
  final = min(final,arms);
  final = min(final,legs);
  final = max(final,-eye);
  return final;
}

float sdScene(vec3 p, out vec3 rot){
  float t = u_time * 1.0;
  float gridX, gridY, grid;
  float thickness = 0.15;

  float cellX = floor(p.x/14.);


  vec3 c = vec3(1,1,0);
  vec3 gp = mod(p,c)-(0.5*c);

  gridX = sdBox(gp+vec3(0.,0., -1.4), vec3(thickness, 1,1));
  gridY = sdBox(gp+vec3(0.,.5, -1.4), vec3(1., thickness, 1.));
  // grid = gridX;
  grid = min(gridY, gridX);
  // gridY = min(gridY, sdBox(gp+vec3(0.,-.5,0), vec3(1., thickness, 1.)));

  float state = step(mod(t, 1.), 0.5);
  state = step(sin(cellX/1. + t*10.), 0.);

  float left = halfInvader(p, state);
  p.x*=-1.;
  float right = halfInvader(p, state);

  float invader = min(left, right);

  // return grid;
  // return min(grid, invader);
  return invader;
  return max(invader, -grid);
}

vec3 estimateNormal(vec3 v){
  vec3 n;
  vec3 dum;
  n.x = sdScene(vec3(v.x + Epsilon, v.y, v.z),dum) - sdScene( vec3(v.x - Epsilon, v.y, v.z),dum);
  n.y = sdScene(vec3(v.x, v.y + Epsilon, v.z),dum) - sdScene( vec3(v.x, v.y - Epsilon, v.z),dum);
  n.z = sdScene(vec3(v.x, v.y, v.z + Epsilon),dum) - sdScene( vec3(v.x, v.y, v.z - Epsilon),dum);
  return normalize(n);
}

float rayMarch(vec3 ro, vec3 rd, out vec3 rot){
  float s = 0.;
  for(int i = 0; i < MaxSteps; i++){
    vec3 p = ro + rd * s;

    float d = sdScene(p, rot);

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

void main(){
  float i;
  float t = u_time * 2.0;

  float dist = 30.;
  // vec3 eye = vec3(cos(0.)*dist, 0, sin(0.)*dist);

  // vec3 eye = vec3(15,5. -t,15);
  vec3 eye = vec3(3.-t,-t,8);
  vec3 center = vec3(-t, -t, 0);
  vec3 up = vec3(0,1,0);
  vec3 lightPos =  vec3(0,0,10);

  vec3 ray = rayDirection(150., u_res, gl_FragCoord.xy);

  vec3 worldDir = viewMatrix(eye, center, up) * ray;
  vec3 rot;
  float d = rayMarch(eye, worldDir, rot);

  vec3 i3;

  if(d < MaxDist){
    vec3 v = eye + worldDir*d;
    vec3 n = estimateNormal(v);
    i3 = vec3(1.) * lighting(v, n, lightPos, eye);
  }


  // i3 = pow(i3, 1./vec3(2.2));
  gl_FragColor = vec4(vec3(i3), 1);
}