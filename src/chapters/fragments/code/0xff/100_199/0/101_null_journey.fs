// 101 - "Null Journey"
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.0001;
const float MaxDist = 200.;
const int MaxSteps = 128;

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
  vec2 d = abs(vec2(length(p.xz),p.y)) - sz;
  float _out = length(max(vec2(d.x,d.y), 0.));
  float _in = min(max(d.x,d.y), 0.);
  return  _in + _out;
}

float lighting(vec3 p, vec3 n, vec3 lightPos){
  vec3 pToLight = vec3(lightPos - p);
  float power = 10.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  
  float nDotL = max(dot(n,lightRayDir), 0.);

  float ambient = .21;
  float diffuse = (nDotL*power) / d;
  
  return ambient + diffuse;
}

vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  vec2 xy = fragCoord - size / 2.0;
  float z = size.y / tan(radians(fieldOfView) / 2.0);
  return normalize(vec3(xy, -z));
}

float sdScene(vec3 p){
	float dist = 0.;
	p.x += ((sin(p.z*3.5 + u_time*10.)+1.)/2.);

	float c = sdCylinder(p, vec2(113., 1.)) - dist;
	float s = sdSphere(p + vec3(-1.5,-1.,-.7), .5);
	
	return s;
	// return min(c, s);
	// return max(c, -s);
	// return c;
}

vec3 estimateNormal(vec3 v){
  vec3 n;
  n.x = sdScene(vec3(v.x + Epsilon, v.y, v.z)) - sdScene( vec3(v.x - Epsilon, v.y, v.z));
  n.y = sdScene(vec3(v.x, v.y + Epsilon, v.z)) - sdScene( vec3(v.x, v.y - Epsilon, v.z));
  n.z = sdScene(vec3(v.x, v.y, v.z + Epsilon)) - sdScene( vec3(v.x, v.y, v.z - Epsilon));
  return normalize(n);
}

float rayMarch(vec3 ro, vec3 rd, out vec3 col){
	float s = 0.;
	for(int i = 0; i < MaxSteps; i++){
		vec3 p = ro + rd * s;
		
		float d = sdScene(p);
		
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
  float stepSize = .019;
  float t = stepSize;
  float oc = 0.;
  for(int i = 0; i < 2; ++i)
  {
    float d = sdScene(p+n*t);
    oc += t - d;
    t += stepSize;
  }

  return clamp(oc, 0., 1.);
}


void main(){
	vec2 p = (gl_FragCoord.xy/u_res)*2. -1.;
	float i;
	vec3 eye = vec3(3, 2.7, 2.);
	vec3 center = vec3(0,0,0);
	vec3 lightPos = eye + vec3(1,2,0);
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
		_ao = ao(v, n);

		float intensity = lighting(v, n, lightPos);
		i = intensity - _ao;
	}

	gl_FragColor = vec4(vec3(i), 1);
}