// 94
precision mediump float;

const int MaxSteps = 100;
const float Epsilon = 0.0001;
const float MaxDist = 100.;
const float  MinDist = .08;

uniform vec2 u_res;
uniform float u_time;

mat4 rotateY(float theta) {
    float c = cos(theta);
    float s = sin(theta);

    return mat4(
        vec4(c, 0, s, 0),
        vec4(0, 1, 0, 0),
        vec4(-s, 0, c, 0),
        vec4(0, 0, 0, 1)
    );
}

float phong(vec3 p, vec3 n, vec3 lightPos){
  vec3 pToLight = vec3(lightPos - p);
  float power = 15.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  
  float nDotL = max(dot(n,lightRayDir), 0.);

  float ambient = .1;
  float diffuse = (nDotL*power) / d;
  
  return ambient + diffuse;
}

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}
float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.  
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;  
  return fract( sin(x+y) * 23454.);
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

float sdBox(vec3 p, vec3 r){
	vec3 d = abs(p) - r;
	float _out = length(max(d,0.));
	float _in = min(max(d.x, max(d.y, d.z)), 0.);
	return _in + _out;
}

float sdSphere(vec3 p, float r){
	return length(p)-r;
}

float sdScene(vec3 p){
	vec3 space = floor(p)*2.-1.;

	float s = .25;
	vec3 np = p;
	np.x = mod(p.x,s)-.5*s;
	np.z = mod(p.z,s)-.5*s;

	// vec2 idx = vec2( floor(p.x+0.5), floor(p.z+0.5));
	// float n = floor(valueNoise(idx)*10.)/10.;
	// float n = valueNoise( vec2(0, .4));
	// float x = abs(cos(p.z));
	// float x = 
	//cos(floor((p.z-.5))/10.);
	// float x = abs(cos(floor(p.z)*.25)*1.);

	// float n = valueNoise( floor(p.xz-0.5) * 5. )/5.;

    // float Xidx = floor( ((p.x*.125)-.125) *5. )/5.;
    // float Yidx = floor( ((p.z*.125)-.125) *5. )/5.;
    // vec2 idx = vec2(Xidx, Yidx);

	
  vec2 cell = floor(p.xz * 1./s);

	float n = smoothValueNoise( cell );

	// np = (vec4(np,1.) * rotateY(si  n(space.z))).xyz;
	return sdBox(np, vec3(0.1, 1. * n, 0.1));
}

vec3 estNormal(vec3 p){
	vec3 n;
	vec3 e = vec3(Epsilon,0,0);
	n.x = sdScene(p + e.xzz) - sdScene(p - e.xzz);
	n.y = sdScene(p + e.zxz) - sdScene(p - e.zxz);
	n.z = sdScene(p + e.zzx) - sdScene(p - e.zzx);
	return normalize(n);
}

float rayMarch(vec3 eye, vec3 ray){
  float start = MinDist;
  float depth = start;

  for (int i = 0; i < MaxSteps; ++i){
    
    vec3 p = eye + (ray*depth);
    
    // float d = sdSphere(p);
    float d = sdScene(p);

    // close enough to surface
    if(d < Epsilon){
      return depth;
    }
    depth += d;

    if(depth >= MaxDist){
      return MaxDist;
    }
  }
  return MaxDist;
}

// float rayMarch(vec3 ro, vec3 rd){
// 	float s = 1.;

// 	for(int i = 0; i < MaxSteps; ++i){
// 		vec3 p = ro + (rd * s);
// 		float d = sdScene(p);

// 		if(d < Epsilon){
// 			return s;
// 		}
// 		s += d;

// 		if(d > MaxDist){
// 			return MaxDist;
// 		}
// 	}
// 	return MaxDist;
// }

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.;
  float z = size.y/tan(radians(fieldOfView)/2.);
  return normalize(vec3(xy, -z));
}

void main(){
	float i = 1.;
	vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
	vec3 l = normalize(vec3(0,4,4));

	vec3 center = vec3(0);
	vec3 up = vec3(0,1,0);
	float mv = u_time*1.;

	vec3 eye = vec3(0, 6., 4. - mv);
	center = vec3(0,0,0.-mv);
	vec3 ray = rayDirection(100.0, u_res, gl_FragCoord.xy);

	mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 worldDir = viewWorld * ray;
  float s = rayMarch(eye, worldDir);

	if(s > MaxDist){
		i = 0.;
	}
	else {
		vec3 n = estNormal(eye + worldDir * s);
		//i = dot(n,l) * 10.;
		vec3 point = eye+worldDir*s;
		vec3 lightPos = vec3(0,2,eye.z+3.);
		i += phong(point, n, lightPos);
	}

	i *= 1./s;

	gl_FragColor = vec4(vec3(i),1);
}