// 93 - Plane Noise
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const int MaxStep = 100;
const float MaxDistance = 100.;
const float Epsilon = 0.0001;

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


float sdSphere(vec3 p, float r){
	return length(p)-r;
}

float sdPlane(vec3 p, vec4 n){
  return dot(p,n.xyz) + n.w;
}

float sdScene(vec3 v){
	float t = u_time*15.;
	float s = sdSphere(v+ vec3(0,sin(t)/2.,0)*2., 1.);
	float p = sdPlane(v+vec3(0,1,0), vec4(0,1,0,0));

  return min(s,p);
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.;
  float z = size.y/tan(radians(fieldOfView)/2.);
  return normalize(vec3(xy, -z));
}

vec3 estNormal(vec3 p){
	vec3 n;
  n.x = sdScene(vec3(p.x + Epsilon, p.y, p.z)) - sdScene(vec3(p.x - Epsilon, p.y, p.z));
  n.y = sdScene(vec3(p.x, p.y + Epsilon, p.z)) - sdScene(vec3(p.x, p.y - Epsilon, p.z));
  n.z = sdScene(vec3(p.x, p.y, p.z + Epsilon)) - sdScene(vec3(p.x, p.y, p.z - Epsilon));
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

    if(s >= MaxDistance){
      return MaxDistance;
    }
  }
  return MaxDistance;
}

// float rayMarch(vec3 ro, vec3 rd){
// 	float s = 0.;
// 	for(int i = 0; i < MaxStep; ++i){
// 		vec3 p = ro + (rd*s);
// 		float d = sdScene(p);

// 		if(d < Epsilon){
// 			return s;
// 		}
// 		s += d;

// 		if(d > MaxDistance){
// 			return MaxDistance;
// 		}
// 	}
// 	return MaxDistance;
// }


mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}


float calcLight(vec3 n, vec3 lightPos, vec3 p){
  vec3 pToLight = vec3(lightPos - p);
  float power = 20.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  
  float nDotL = max(dot(n,lightRayDir), 0.);

  float ambient = .2;
  float diffuse = (nDotL*power) / d;
  
  return ambient + diffuse;
}

// float calcLight(vec3 n, vec3 l, vec3 p){
// 	vec3 pToLight = vec3(l - p);
// 	float d = length(pToLight);

// 	float nDotL = clamp(dot(n,normalize(pToLight)), 0., 1.);
	
// 	float dist = d*d;
// 	float power = 20.;
// 	return (nDotL*power) /(dist);
// }

void main(){
	float i = 0.;
	vec2 p = (gl_FragCoord.xy/u_res) * 2. -1.;
	vec3 l = vec3(0,4,3);
	// vec3 ro = vec3(p,5);
	vec3 rd = rayDirection(105.0, u_res, gl_FragCoord.xy);


  vec3 center = vec3(0,0,-4);
  vec3 up = vec3(0,1,0);
  vec3 eye = vec3(0, 3, 6);
  mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 worldDir = viewWorld * rd;


	float s = rayMarch(eye, worldDir);
	vec3 samplePoint = eye + (worldDir*s);
	vec3 n = estNormal(samplePoint);
	float intensity = calcLight(n, l, samplePoint);
	
	i = intensity;
	if(s == MaxDistance){
		i = 0.;
	}


	i *= 1./(pow(s, .35));

	gl_FragColor = vec4(vec3(i),1);
}