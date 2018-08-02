// 121 - "Lost in the Catacombs"
precision highp float;

uniform vec2 u_res;
uniform float u_time;

const int MaxShadowStep = 80;
const float Epsilon = 0.001;
const float MaxDist = 400.;
const int MaxSteps = 128;

const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI*0.5;

const vec3 lightGrey = vec3(1.);
const vec3 darkGrey = vec3(0.4);

float sdRect(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.0) + length(max(d,0.0));
}

float sdCylinder(vec3 p, vec2 sz ){
  vec2 d = abs(vec2(length(p.xz),p.y)) - sz;
  float _out = length(max(vec2(d.x,d.y), 0.));
  float _in = min(max(d.x,d.y), 0.);
  return  _in + _out;
}
float sdCircle(vec2 p, float r){
  return length(p) - r;
}

float samplePac(vec2 p){
  float t = u_time*4.;
  p *= 3.0;

  float theta = abs(atan(p.y,p.x))/PI;
  float i = smoothstep(0.01,0.001,sdCircle(p,.45)) * 
        step(.25,theta+(sin(t*PI*1.)+1.)/2.*.25);
  
  // i += step(sdCircle(p+vec2(mod(t*.5,1.)-1.,0.),.08),0.);

  return i;
}

 

float sdScene2d(vec2 p){
	float res = 1.;
	float t = u_time * .3;

	float rad = .07;
	
  const float NumCols = 5.;
	for(float it = 0.; it < NumCols; ++it){

		float x = mod( (it/NumCols)*2. + t, 2. + rad*2.) - 1. - rad;

		float cTop = sdCircle(p + vec2(x,  .3 ), rad);	
		float cBot = sdCircle(p + vec2(x, -.3), rad);	

		res = min(res, cBot);
		res = min(res, cTop);
	}

	return res;
}

float shadowMarch2d(vec2 p, vec2 l){
	vec2 rd = normalize(l-p);
	vec2 ro = p;

	float s = 0.;
	for(int it = 0; it < MaxShadowStep; it++){
		vec2 v = ro + (rd*s);
		float d = sdScene2d(v);		

		// if we went father than the distance form point to light
		if( length(rd*s) > length(l-p) ){
			return 1.;
		}

		if(d < Epsilon){
			return 0.;
		}
		s += d;
	}

	return 1.;
}

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

float sdSceneRender(vec2 p){
	return step(sdScene2d(p), 0.);
}






////////////


float sampleChecker(vec2 c) {
  float col;
  float sz = 0.85 + 0.15;//*sin(u_time/5.);

  float x = step(mod(c.x,sz), sz/2.);
  float y = step(mod(c.y,sz), sz/2.);

  if(x == y){return 0.8;}
  return x*y;
}

float lighting(vec3 p, vec3 n, vec3 lightPos, vec3 eye){
  float ambient = 0.1;
  // ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 50.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / (d*d);
  float kd = 0.85;

  float gloss = 200.;    
  vec3 V = normalize(eye - p);
  vec3 R = normalize(reflect(-lightPos, n)); 
  float dotRV = dot(R, V);
  float spec = pow(dotRV, gloss);
  float ks = 1.;

  return ambient + kd*diffuse + ks*spec;
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, -z));
}


// sss - just a marker
float sdScene(vec3 p, out float col){
  // float h = getHeight(p.xz);
  // h = max(h, .25);
  // float y = h/4.;
  

  float c_ = 1.0;
  vec2 uv = mod(p.xz, vec2(c_))*0.5*c_ - 0.25; 
  uv *= 4.;
  // uv.x += 0.5;// + sin(u_time);
  // uv.y -= 0.5;


  float ch = sampleChecker(p.xz*4.);
  // float shadowTex = sample2dShadowTex(p.xz);
  // col = ch * shadowTex;
  // col = shadowTex;


  float box = sdBox(p - vec3(0, 0, 0), vec3(1, 0.1, 1.));

  // float test = sdBox(p , vec3(0.5, .5, 0.5));

  float test = sdCylinder(p, vec2(0.1, 1.));

  return min(test, box);
  return box;
}

float shadowMarch(vec3 point, vec3 lightPos){

  vec3 pToLight = lightPos-point;
  vec3 rd = normalize(pToLight);
  vec3 ro = point;

  float s = 0.;
  for(int i = 0; i < MaxShadowStep; ++i){
    vec3 v = ro + (rd*s);

    float dum;
    float dist = sdScene(v, dum);

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

// void main(){
//   float i;
//   float t = u_time * 0.;
//   vec2 fc = gl_FragCoord.xy;

//   float dist = 2.;
//   vec3 eye = vec3(dist * cos(t), 3.  , dist * sin(t));
//   // vec3 eye = vec3(2., 2., 2.);
//   vec3 center = vec3(0, 0, 0.);
//   vec3 lightPos =  vec3(2., 2., 2.);
//   vec3 up = vec3(0,1,0);

//   mat3 viewWorld = viewMatrix(eye, center, up);
//   vec3 ray = rayDirection(90., u_res, fc);

//   vec3 worldDir = viewWorld * ray;
//   vec3 col;
//   float d = rayMarch(eye, worldDir, col);
//   vec3 v = eye + worldDir*d;

//   if(d < MaxDist){
//         // float visibleToLight = shadowMarch(eye+worldDir*(d-0.01), lightPos);
//     // vec3 n = estimateNormal(v);

//     // float lights = lighting(v, n, lightPos, eye);
//     // i = lights * col.x;
//     i = col.x;
//     // i = col.x;

//     // if(visibleToLight == 0.){
//       // i -= .5;
//       // i = 0.;
//     // }
//   }
  
//   // i = pow(i, 1./2.2);
//   gl_FragColor = vec4(vec3(i), 1);
// }


void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;

  vec2 lightPos = vec2(0.);
  //vec2(sin(u_time*2.)*0.5, sin(u_time*3.)*.3);

  float i = sdSceneRender(p);
  // i -= 0.4;

  // draw light
  // i -= step(sdCircle(p-lightPos, 0.01), 0.);
  i = samplePac(p);

  float visibleToLight = shadowMarch2d(p, lightPos);
  // i -= step(visibleToLight, 0.) *.84;
  if(visibleToLight == 0.){
    i = 0.72;
  }

  gl_FragColor = vec4(vec3(i),1);
  // return i;
}
