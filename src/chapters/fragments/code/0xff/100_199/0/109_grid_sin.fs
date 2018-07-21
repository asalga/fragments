// 109 - The old classic square sin set
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.0001;
const float E = Epsilon;
const float MaxDist = 400.;
const int MaxSteps = 128;
const float PI = 3.141592658;
const float HALF_PI = PI*0.5;
const int MaxShadowStep = 100;

const vec3 lightGrey = vec3(1.);
const vec3 darkGrey = vec3(0.4);
const float X_SCALE= 13443.;
const float Y_SCALE = 389492.;

float sdSphere(vec3 p, float r){
	return length(p)-r;
}
float valueNoise(float seed, vec2 p){  
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  return fract( sin(x+y) * (23454. + seed));
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
	float ambient = .2;
	// ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 70.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  // ---
  // float gloss = 30.;
  // vec3 H = normalize(lightRayDir + p);
  // float NdotH = dot(n, H);
  // vec3 r = reflect(lightRayDir, n);
  // float RdotV = dot(r, normalize(p));
  // float spec = pow( NdotH , gloss );
  // float spec = pow(RdotV, gloss) / d;
  // ---
  return ambient + diffuse;// + spec;
}

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

// sss - just a marker
float sdScene(vec3 p, out float col){
	col = .4;
	float d;
	float t = u_time * 5.;

	float c = 1.;
	vec3 np = mod(p, vec3(c))-c *0.5;
	np.y = p.y;
	
	// float xIdx = abs(sin( floor(p.x)/10. + t )) *3.;
	float xIdx = floor(p.x)/10.;
	float zIdx = floor(p.z)/10.;

	float cnt = 9.;
	if( abs(p.x) >= cnt || abs(p.z) > cnt ){
		col =  0.;
		return MaxDist;
	}

	float len = sin(length( vec3(9.) * vec3(xIdx, 0, zIdx)) - t) / 8.;
	vec3 off = vec3(0, -len ,0);
	d = cubeSDF( (np + off) , vec3(0.35, 2. + len*1., .35));

	return d;
}


float ao(vec3 p, vec3 n)
{
  float stepSize = .1;
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
		s += d/1.1;

		if(d > MaxDist){
			return MaxDist;
		}
	}
	return MaxDist;
}



void main(){
	// vec2 p = (gl_FragCoord.xy/u_res)*2. -1.;
	float i;
	float t = u_time*1.;
	vec2 fc = gl_FragCoord.xy;

	float x =  sin(t) * 10.;
	float z =  cos(t) * 10.;
	z = 5.;
	x = 5.;

	vec3 eye = vec3( 8.  , 5. , 8.);
	vec3 center = vec3(0);
	vec3 lightPos =  vec3(8,10,8);
	//  vec3(-1,5,-1) + eye;
	vec3 up = vec3(0,1,0);

	mat3 viewWorld = viewMatrix(eye, center, up);
	vec3 ray = rayDirection(70.0, u_res, fc);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float d = rayMarch(eye, worldDir, col);

  vec3 v = eye + worldDir*d;

	if(d < MaxDist){
		vec3 n = estimateNormal(v);
		float _ao = ao(v, n);
		float lights = lighting(v, n, lightPos);
		 i += (lights/5.) * (1.-_ao)*10.;
	}

	gl_FragColor = vec4(vec3(i), 1);
}