// 105 - "Cube Walk"
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.0001;
const float E = Epsilon;
const float MaxDist = 100.;
const int MaxSteps = 228;
const float PI = 3.141592658;
const float HALF_PI = PI*0.5;
const int MaxShadowStep = 100;

const vec3 lightGrey = vec3(1.);
const vec3 darkGrey = vec3(0.4);

float sdSphere(vec3 p, float r){
  return length(p)-r;
}

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}

float sdCylinder(vec3 p, vec2 sz ){
  vec2 d = abs(vec2(length(p.xy),p.z)) - sz;
  float _out = length(max(vec2(d.x,d.y), 0.));
  float _in = min(max(d.x,d.y), 0.);
  return  _in + _out;
}




float lighting(vec3 p, vec3 n, vec3 lightPos, vec3 eye){
  float ambient = 0.3;
  // ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 10.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  float kd = 1.;

  float gloss = 10.;
  vec3 V = normalize(eye - p);
  vec3 R = normalize(reflect(-lightPos, n));
  float dotRV = dot(R, V);
  float spec = pow(dotRV, gloss);

  return ambient +
          diffuse * 1.5 +
          spec * 1.4;
}


// float lighting(vec3 p, vec3 n, vec3 lightPos){
//   float ambient = .5;
//   // ---
//   vec3 pToLight = vec3(lightPos - p);
//   float power = 30.;
//   vec3 lightRayDir = normalize(pToLight);
//   float d = length(pToLight);
//   d *= d;
//   float nDotL = max(dot(n,lightRayDir), 0.);
//   float diffuse = (nDotL*power) / d;
//   // ---
//   // float gloss = 30.;
//   // vec3 H = normalize(lightRayDir + p);
//   // float NdotH = dot(n, H);
//   // vec3 r = reflect(lightRayDir, n);
//   // float RdotV = dot(r, normalize(p));
//   // float spec = pow( NdotH , gloss );
//   // float spec = pow(RdotV, gloss) / d;
//   // ---
//   return ambient + diffuse;// + spec;
// }

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, -z));
}

float cubeSDF(vec3 p, vec3 sz) {
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

float menger(vec3 p){

  float res = cubeSDF(p, vec3(1.0));

  float s = 1.0;
  for(int it = 0; it < 3; ++it){

    vec3 a = mod(p*s, 2.) - 1.0; // -1   1
    s *= 3.0;
    vec3 r = abs(1.0 - 3.0*abs(a)); // 1-   (  -3   3)

    float da = max(r.x,r.y);
    float db = max(r.y,r.z);
    float dc = max(r.z,r.x);

    float c = (min(da,min(db,dc))-1.0)/s;

    if( c > res ){
      res = c;
    }
  }

  return res;

}

float sdScene(vec3 p, out float col){
  // float s = sdSphere(p, 1.);


  float t = u_time*2.;// * abs(sin(u_time)+1.)/2.;
  vec3 op = p;

  p -= vec3(2,0,0);
  vec3 moveBack = vec3( mod( t*2., 2. ) ,0,0);

  vec4 p4 = vec4(p+moveBack, 1.);
  vec3 np = (p4 * r2dZ( mod(t*HALF_PI, HALF_PI) )).xyz;

  vec3 _ = vec3(1.1, .85, 0);
  float hole1 = cubeSDF(np + vec3(1,-1.,0), vec3(_.xyy));
  float hole2 = cubeSDF(np + vec3(1,-1.,0), vec3(_.yyx));
  float hole3 = cubeSDF(np + vec3(1,-1.,0), vec3(_.yxy));




  float c = menger(np + vec3(1,-1.,0));

  // ?max(cubeSDF(np + vec3(1,-1.,0), vec3(1.-E)), -hole1);
   // = max(c, -hole2);
  // c = max(c, -hole3);
  // c = min(c, sdSphere(p - vec3(-1., 1, 0), 0.8));


  float ground = cubeSDF(op, vec3(40,.01,40));

  if(c < E){
    col = lightGrey.x;
  }
  else{
    float X = step(mod(p.x + moveBack.x, 1.), 0.5);
    float Y = step(mod(p.z, 1.), 0.5);

    float c = X+Y;
    if(c == 2.){c = 0.;}
    col = c;
  }

  return min(c, ground);
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
  float s = 0.;
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

float ao(vec3 p, vec3 n)
{
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
  vec2 p = (gl_FragCoord.xy/u_res)*2. -1.;
  float i;
  float t = u_time*1.;

  float x =  sin(t) * 5. + (sin(t)+1.)/2.*5. ;
  float z =  cos(t) * 10.;

  vec3 eye = vec3( x, 5. + sin(t*4.)*0.5, z);
  vec3 center = vec3(0,1,0);
  vec3 lightPos =   vec3(10,7,5);
  vec3 up = vec3(0,1,0);

  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 ray = rayDirection(80.0, u_res, gl_FragCoord.xy);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float d = rayMarch(eye, worldDir, col);


  i = 0.1;
  vec3 v = eye + worldDir*d;

  if(d < MaxDist){
    float visibleToLight = shadowMarch(eye+worldDir*(d-0.001), lightPos);
    vec3 n = estimateNormal(v);
    float lights = lighting(v, n, lightPos,eye);

      i += d * lights * col.x;

    if(visibleToLight == 0.){
      i *= 0.4;
    }

  }

  float fog = 5./ pow( d, 2.);
  i *= fog;


  // float fog = 1./pow(d*1., .01);
  // i *= fog;

  // if(mod(gl_FragCoord.y, 3.) < 2.){
    // i *= .5;
  // }

  // float vignette = 1.-smoothstep(0.9, 1., abs(p.x)) *
                   // 1.-smoothstep(0.9, 1., abs(p.y));
  // i *= vignette;

  gl_FragColor = vec4(vec3(i), 1);
}