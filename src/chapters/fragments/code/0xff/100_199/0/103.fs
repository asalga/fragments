// 103 - "Sinlinder"
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.001;
const float E = Epsilon;
const float MaxDist = 10.;
const int MaxSteps = 228;

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

float lighting(vec3 p, vec3 n, vec3 lightPos){
	float ambient = .1;
	// ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 3.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  // ---
  // float gloss = 0.;
  // vec3 H = normalize(lightRayDir + p);
  // float NdotH = dot(n, H);
  // vec3 r = reflect(lightRayDir, n);
  // float RdotV = dot(r, normalize(p));
  // float spec = pow( NdotH , gloss );
  // float spec = pow(RdotV, gloss);
  // ---
  return ambient + diffuse;// + spec;
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, -z));
}

float sdScene(vec3 p, out float col){
	float dist = 0.;
	float t = u_time;
	// t=0.;
	// p.x += ((sin(p.z*2.5 + u_time*10.)+1.)/2.);

	float d = 0.5;
	float x = step(d/2., mod(p.z - t , d));
	float y = step(d/2., mod(p.x,  d));

	col = lightGrey.x;

	if(x == y){
		col = darkGrey.x;
	}

	float disp = 0.;
	// disp = mod(p.z*12., .5);
	float test = atan(p.y, p.x);
	disp = (sin(t + test * 10.)+1.)/2. + 0.25;
	disp /= 10.;

	// float disp = (sin(u_time)*p.x +1.)/2.;
	// disp *= col;
	// disp *= 2.;

	// disp *= smoothstep(0.2, .5, col/1.);
	// disp *=2.;

	//float ss = smoothstep(0.1, .01, col/1.);
	//float ss = col/2.;
	
	// vec3 mv = vec3(sin(0),0,0);
	float c = sdCylinder(p , vec2(1.-E, 60)) + disp;

	// float s = sdSphere(p + vec3(0,0,sin(t*2.)), 1.) - disp;

	// return c;
	return c;
	// return mix(c,s, (sin(-u_time*0.)+1.)/2.);
	// return min(c, s);
	// return max(c, -s);
	// return c;
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
	float t = u_time;


	// float test = atan(p.y, p.x);
	// disp = (sin(t*1. + test*20.)+1.)/2. + 0.25;
	// disp /= 10.;
	float st = (sin(t)+1.)/64.;
	vec3 eye = vec3(0., st + 1., 0.);
	vec3 center = vec3(0,0,-10);
	vec3 lightPos = eye + vec3(1,2,3);
	vec3 up = vec3(0,1,0);

	mat3 viewWorld = viewMatrix(eye, center, up);
	vec3 ray = rayDirection(80.0, u_res, gl_FragCoord.xy);

  	vec3 worldDir = viewWorld * ray;
  	vec3 col;
  	float d = rayMarch(eye, worldDir, col);

	if(d < MaxDist){
		vec3 v = eye + worldDir*d;
		vec3 n = estimateNormal(v);

		float _ao;
		// _ao = ao(v, n);

		float lights = lighting(v, n, lightPos);
		i = col.x * lights - _ao;
		
		// i = intensity - _ao;
		float fog = 1./pow(d, 1.);
		i *= fog;
	}

	if(mod(gl_FragCoord.y, 2.) < 1.){
		// i /= 4.8;
		i = 0.;
	}

	float vignette = 1.-smoothstep(0.9, 1., abs(p.x)) *  
									 1.-smoothstep(0.9, 1., abs(p.y));
	i *= vignette;

	gl_FragColor = vec4(vec3(i), 1);
}