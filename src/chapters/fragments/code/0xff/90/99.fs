// 90 - Space Roads
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.14159265834
#define TAU (PI*2.)
const float SPEED = 1.1;

const int MaxStep = 200;
const int MaxShadowStep = 100;
const float MaxDist = 50.;
const float MinDist = -0.;
const float Epsilon = 0.001;

const vec3 lightGrey = vec3(1.);
const vec3 darkGrey = vec3(0.4);

float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.  
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;  
  return fract( sin(x+y) * 23454.);
}

float xor(float a, float b){
	return (a+b == 1.) ? 1. : 0.;
}

float smoothValueNoise(vec2 p){
  vec2 lv = fract(p);
  lv = smoothstep(0.,1.,lv);
  vec2 id = floor(p);
  float bl = valueNoise(vec2(id));
  float br = valueNoise(vec2(id)+vec2(1,0));
  float b = mix(bl,br,lv.x);

  float tl = valueNoise(vec2(id)+vec2(0,1));
  float tr = valueNoise(vec2(id)+vec2(1,1));
  float t = mix(tl,tr,lv.x);

  return mix(b,t,lv.y);
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.;
  float z = size.y/tan(radians(fieldOfView)/2.);
  return normalize(vec3(xy, -z));
}

float cubeSDF(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz;
  float insideDistance = min(max(d.x, max(d.y, d.z)), 0.);
  float outsideDistance = length(max(d, 0.));
  return insideDistance + outsideDistance;
}

float sdSphere(vec3 p, float r){
	return length(p) - r;
}


float sdScene(in vec3 p, out vec3 col){
	float sz = 1.;
	vec3 offset = vec3(0, 0.5, 0);
	float t = u_time * SPEED;

	vec3 np = p;
	np.x = mod(p.x, sz) -  .5*sz;
	np.z = mod(p.z, sz) -  .5*sz;

	// float n = valueNoise(vec2(0., step(.5, mod(np.z, 1.))  ) );
	vec3 cell = floor(p) * 2.;
	float n = valueNoise(cell.zx);
	// n = step(0.5, n);

	float holes = cubeSDF(np + vec3(0, 0, 0),  vec3(.5001, .2*n, .5001));

	// np = p;

	float x = step(mod(p.x, sz), sz * .5 );
	float y = step(mod(p.z, sz), sz * .5 );

	float road = cubeSDF(np - vec3(0,  sign(sin( (p.z/1.) * PI )) * n * 0.01  , 0), vec3(.5, .01, .5));

	// Road checkerboard TODO: fix
	float colId = xor(x,y);
	col = darkGrey;
	if(colId == 0.) col = lightGrey;

	if(n == 0.){
		// col *= valueNoise(cell.zx) * np.x;
	}
	

	return road;
	// return n;
	// return min(road, holes);
	// return holes;
	return max(road, -holes);
}

vec3 estimateNormal(vec3 v){
  vec3 n;
  vec3 dum;
  n.x = sdScene(vec3(v.x + Epsilon, v.y, v.z), dum) - sdScene( vec3(v.x - Epsilon, v.y, v.z), dum);
  n.y = sdScene(vec3(v.x, v.y + Epsilon, v.z), dum) - sdScene( vec3(v.x, v.y - Epsilon, v.z), dum);
  n.z = sdScene(vec3(v.x, v.y, v.z + Epsilon), dum) - sdScene( vec3(v.x, v.y, v.z - Epsilon), dum);
  return normalize(n);
}

float rayMarch(vec3 ro, vec3 rd, out vec3 col){
  float s = MinDist;
  for(int i = 0; i < MaxStep; ++i){
    vec3 v = ro + (rd*s);
    
    float dist = sdScene(v, col);

    if(dist < Epsilon){
      return -1.;
    }
    s += dist/4.;

    if(s >= MaxDist){
      return MaxDist;
    }
  }
  return MaxDist;
}

// float shadowMarch(vec3 point, vec3 lightPos){

//   vec3 pToLight = lightPos-point;
//   vec3 rd = normalize(pToLight);
//   vec3 ro = point;

//   float s = 0.;
//   for(int i = 0; i < MaxShadowStep; ++i){
//     vec3 v = ro + (rd*s);
//     vec3 dum;
//     float dist = sdScene(v, dum);

//     if(dist < Epsilon){
//       return 0.;
//     }
//     s += dist;

//     if(s >= MaxDist){
//       return 1.;
//     }
//   }
//   return 1.;
// }

float phong(vec3 p, vec3 n, vec3 lightPos){
  vec3 pToLight = vec3(lightPos - p);
  float power = 35.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  
  float nDotL = max(dot(n,lightRayDir), 0.);

  float ambient = .0;
  float diffuse = (nDotL*power) / d;
  return ambient + diffuse;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  vec3 i;
  float t = u_time * SPEED;
  vec3 eye = vec3( 0., 1.0, 2. - t*2.);
  vec3 ray = rayDirection(140.0, u_res, gl_FragCoord.xy);
  vec3 lightPos = eye +
  vec3(0, 5, 1);

  vec3 col;
  float d = rayMarch(eye, ray, col);
 

  if(d < MaxDist){
  	// vec3 point = eye+ray*(d);
  	// vec3 n = estimateNormal(point);
		i += col;// * phong(point, n, lightPos);
		// i += phong(point, n, lightPos);
	}

	float fog = 1./pow(d, .15);
	i *= fog;

  gl_FragColor = vec4(vec3(i),1);
}