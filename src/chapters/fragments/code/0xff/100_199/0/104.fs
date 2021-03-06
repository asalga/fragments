// 104
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.001;
const float E = Epsilon;
const float MaxDist = 10.;
const int MaxSteps = 228;
const float PI = 3.141592658;

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
	float ambient = .05;
	// ---
  vec3 pToLight = vec3(lightPos - p);
  float power = 30.;
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


float sdScene(vec3 p, out float col){
	float s = sdSphere(p, 1.);
	float t = u_time * 6.;

	mat4 topRotMat = r2dZ(abs(cos(t*2.)));
	mat4 botRotMat = r2dZ(-abs(cos(t*2.)));

	vec3 topRotp = vec3(topRotMat * vec4(p, 1.));
	vec3 botRotp = vec3(botRotMat * vec4(p, 1.));

  float ctop = cubeSDF(topRotp + vec3(0,1,0), vec3(1));
  float cbot = cubeSDF(botRotp - vec3(0,1,0), vec3(1));

	float top = max( sdSphere(p, .9) , -ctop);
	float bot = max( sdSphere(p, .9) , -cbot);

	// return min(ctop, cbot);
	// float bottom = sdSphere(p, 1.);
	// return ctop;
	// float res = min( max(top, -ctop), max(bottom, -cb));
	//, + cubeSDF(p+vec3(0,0.5,0), vec3(1)) );

	// food
	vec3 repP = mod(p + t*1.0 - 0.3, PI*0.5) - 0.5;
	repP.z = p.z;
	repP.y = p.y;
	if( p.x < 0.){
		repP.x = p.x;
	}

	float food = cubeSDF(repP, vec3(0.125));
	return min(food, min(top, bot));
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
		s += d/1.;

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

	float x =  + sin(t*1.) * .2;
	vec3 eye = vec3(5. + x  , 1.2, 6. + sin(t*1.) * 2.);
	vec3 center = vec3(2,0,0);
	vec3 lightPos =   vec3(20,0,0);
	vec3 up = vec3(0,1,0);

	mat3 viewWorld = viewMatrix(eye, center, up);
	vec3 ray = rayDirection(80.0, u_res, gl_FragCoord.xy);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float d = rayMarch(eye, worldDir, col);


  i = 0.1;
	if(d < MaxDist){
		vec3 v = eye + worldDir*d;
		vec3 n = estimateNormal(v);

		float lights = lighting(v, n, lightPos);
		i += d * lights;
		
		// i = intensity - _ao;
		// float fog = 1./pow(d, 1.);
		// i *= fog;
	}

	if(mod(gl_FragCoord.y, 3.) < 2.){
		i *= .5;
	}

	float vignette = 1.-smoothstep(0.9, 1., abs(p.x)) *  
									 1.-smoothstep(0.9, 1., abs(p.y));
	i *= vignette;

	gl_FragColor = vec4(vec3(i), 1);
}