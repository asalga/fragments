// 94
precision mediump float;

const int MaxSteps = 128;
const float Epsilon = 0.001;
const float MaxDist = 100.;

uniform vec2 u_res;
uniform float u_time;

mat3 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat3(s, u, -f);
}

float sdSphere(vec3 p, float r){
	return length(p)-r;
}

float sdScene(vec3 p){
	float s = .5;
	vec3 np = mod(p,s)-vec3(0.5, 0.5, 0.5)*s;
	return sdSphere(np, .125);
}

vec3 estNormal(vec3 p){
	vec3 n;
	vec3 e = vec3(Epsilon,0,0);
	n.x = sdScene(p + e.xzz) - sdScene(p - e.xzz);
	n.y = sdScene(p + e.zxz) - sdScene(p - e.zxz);
	n.z = sdScene(p + e.zzx) - sdScene(p - e.zzx);
	return normalize(n);
}

float rayMarch(vec3 ro, vec3 rd){
	float s;

	for(int i = 0; i < MaxSteps; ++i){
		vec3 p = ro + (rd * s);
		float d = sdScene(p);

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
	vec3 eye = vec3(0,0,4. - u_time);
	center = eye + vec3(0,0,-3);
	vec3 ray = rayDirection(100.0, u_res, gl_FragCoord.xy);

	mat3 viewWorld = viewMatrix(eye, center, up);
  vec3 worldDir = viewWorld * ray;
  float s = rayMarch(eye, worldDir);

	if(s > MaxDist){
		i = 0.;
	}
	else {
		vec3 n = estNormal(eye + worldDir * s);
		i = dot(n,l);
	}


	i *= 1./s;

	gl_FragColor = vec4(vec3(i),1);
}