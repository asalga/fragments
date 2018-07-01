precision highp float;

uniform vec2 u_res;
uniform float u_time;

const int MaxSteps = 400;
const float MaxDist = 1000.;
const float Epsilon = 0.00001;

float sdSphere(vec3 v, float r){
	return length(v)-r;
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
    vec2 xy = fragCoord - size / 2.0;
    float z = size.y / tan(radians(fieldOfView) / 2.0);
    return normalize(vec3(xy, -z));
}


float sdBox(vec3 p, vec3 sz){

	vec3 d = abs(p) - sz;
	float inside = min(max(d.x,max(d.y,d.z)), 0.);
	float outside = length(max(d,0.));
	return inside + outside;
}

mat4 rotateY(float theta) {
  float c = cos(theta);
  float s = sin(theta);

  return mat4(
      vec4(c,  0, s, 0),
      vec4(0,  1, 0, 0),
      vec4(-s, 0, c, 0),
      vec4(0,  0, 0, 1)
  );
}

float sdScene(vec3 v){
	vec3 nv = (vec4(v,1)*rotateY(u_time*0.)).xyz;

	nv.x = fract(nv.x);  
	nv.z = fract(nv.z);  

	float s = sdSphere(nv - vec3(.5, -1.8, 0.),.4);

	return s;
	float c = sdBox(nv-vec3(0.25, 0.23, 0.25), vec3(.25));

	vec3 test = vec3(-0.2) * step(mod(u_time,2.), 1.) * fract((u_time*1.)/2.)*8.; 

	// float c_ = max(
		// sdSphere(nv - vec3(.5,.5, 0.) + test ,.4),
		// sdBox(nv - vec3(.5,.5, 0.) + vec3(0.2, 0.23, 0.25) + test, vec3(.25)));

	// float c1 = max(s,c);
	// sdBox(nv-vec3(0.25, 0.23, 0.25), vec3(.25));
	// return c_;
	// return min(max(s, -c), c_);
}



vec3 estimateNormal(vec3 v){
	vec3 n;
	n.x = sdScene(vec3(v.x + Epsilon, v.y, v.z)) - sdScene( vec3(v.x - Epsilon, v.y, v.z));
	n.y = sdScene(vec3(v.x, v.y + Epsilon, v.z)) - sdScene( vec3(v.x, v.y - Epsilon, v.z));
	n.z = sdScene(vec3(v.x, v.y, v.z + Epsilon)) - sdScene( vec3(v.x, v.y, v.z - Epsilon));
	return normalize(n);
}

float rayMarch(vec3 eye, vec3 ray){
	float s = 0.;
	for(int i = 0; i < MaxSteps; ++i){
		vec3 v = eye + (ray*s);
		float d = sdScene(v);

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
	vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;

	vec3 eye = vec3(vec2(p),1. - u_time*u_time*u_time);
	// vec3 ray = normalize(  vec3(vec2(p)*.02 ,-1)  );
	vec3 ray = rayDirection(130.0, u_res, gl_FragCoord.xy);

	vec3 dirLight = normalize(vec3(sin(u_time*0.)*2.,3,4));

	float d = rayMarch(eye, ray);

	vec3 n = estimateNormal(eye+d*ray);

	float nDotL = max(dot(dirLight,n), 0.);
	i = nDotL + 0.13;
	// i = d;

	if(d == MaxDist){
		// i = 0.;
	}

	// float fog = 1./pow(0.71,d);
	float fog = pow(1./d, .391);
	i *= fog;

	gl_FragColor = vec4(vec3(i),1);
}