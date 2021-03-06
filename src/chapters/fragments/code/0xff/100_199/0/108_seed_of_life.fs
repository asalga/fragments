// 108 - "of life"
precision highp float;
uniform vec2 u_res;
uniform float u_time;

const float Epsilon = 0.0001;
const float E = Epsilon;
const float MaxDist = 100.;
const int MaxSteps = 228;
const int MaxShadowStep = 100;

const float PI = 3.141592658;
const float HALF_PI = PI*0.5;
const float TAU = PI*2.;

// polynomial smooth min (k = 0.1);
// from our good friend iq
float smin( float a, float b, float k )
{
    float h = clamp( 0.5+0.5*(b-a)/k, 0.0, 1.0 );
    return mix( b, a, h ) - k*h*(1.0-h);
}

float sdSphere(vec3 p, float r){
	return length(p)-r;
}

float sdTorus( vec3 p, vec2 t){
  vec2 q = vec2(length(p.xz)-t.x, p.y);
  return length(q) - t.y;
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
  float power = 100.;
  vec3 lightRayDir = normalize(pToLight);
  float d = length(pToLight);
  d *= d;
  float nDotL = max(dot(n,lightRayDir), 0.);
  float diffuse = (nDotL*power) / d;
  // ---
  float gloss = 1.;
  vec3 H = normalize(lightRayDir + p);
  float NdotH = dot(n, H);
  // vec3 r = reflect(lightRayDir, n);
  // float RdotV = dot(r, normalize(p));
  float spec = pow( NdotH , gloss );
  // float spec = pow(RdotV, gloss) / d;
  // ---
  return ambient + diffuse + spec;
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

mat4 r2dX(float a){
	float c = cos(a);
	float s = sin(a);
	return mat4(1,  0,  0,  0,
							0,  c, -s,  0,
							0,  s,  c,  0,
							0,  0,  0,  1);
}



float sdScene(vec3 p, out float col){
	float d = 1.00;
	float t = u_time * 0.1;

	const float r = TAU/6.;
	const float sz = .8;
	const float offset = sz;
	const float th = 0.02;
	float k = 0.1;

	// as each approached camera, quickly 
	// transition 

	float c = 1.15;
	p.z = mod(p.z, c) - c * 0.5;

	vec3 _0 = (vec4(p,1.) * r2dX(PI/2.)).xyz + offset * vec3(cos(r*1.), 0, sin(r*1.));
	d =         sdTorus( _0, vec2(sz, th));

	vec3 _1 = (vec4(p,1.) * r2dX(PI/2.)).xyz + offset * vec3(cos(r*2.), 0, sin(r*2.));
	d = smin(d, sdTorus( _1, vec2(sz, th)), k);

	vec3 _2 = (vec4(p,1.) * r2dX(PI/2.)).xyz + offset * vec3(cos(r*3.), 0, sin(r*3.));
	d = smin(d, sdTorus( _2, vec2(sz, th)), k);

	vec3 _3 = (vec4(p,1.) * r2dX(PI/2.)).xyz + offset * vec3(cos(r*4.), 0, sin(r*4.));
	d = smin(d, sdTorus( _3, vec2(sz, th)), k);

	vec3 _4 = (vec4(p,1.) * r2dX(PI/2.)).xyz + offset * vec3(cos(r*5.), 0, sin(r*5.));
	d = smin(d, sdTorus( _4, vec2(sz, th)), k);

	vec3 _5 = (vec4(p,1.) * r2dX(PI/2.)).xyz + offset * vec3(cos(r*6.), 0, sin(r*6.));
	d = smin(d, sdTorus( _5, vec2(sz, th)), k);

	vec3 _6 = (vec4(p,1.) * r2dX(PI/2.)).xyz;
	d = smin(d, sdTorus( _6, vec2(sz, th)), k);

	return d;
}


float shadowMarch(vec3 point, vec3 lightPos){

  vec3 pToLight = lightPos-point;
  vec3 rd = normalize(pToLight);
  vec3 ro = point;

  float s = 0.;
  for(int i = 0; i < MaxShadowStep; ++i){
    vec3 v = ro + (rd*s);
    
    float dum;
    float dist;
    dist = sdScene(v, dum);

    if(dist < Epsilon){
      return 0.;
    }
    s += dist/1.;

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
	float t = u_time*2.;

	float x = sin(t) * 10.;
	float z = cos(t) * 10.;

	// vec3 eye = vec3( x  , 5. + sin(t*4.)*0.5, z);
	vec3 eye = vec3(1, 1, 10. + t);

	vec3 center = vec3(-2, -1, eye.z+ 10.);
	vec3 lightPos =  vec3(0,0,-5) + eye;
	vec3 up = vec3(0,1,0);

	mat3 viewWorld = viewMatrix(eye, center, up);
	vec3 ray = rayDirection( 120.0, u_res, gl_FragCoord.xy);

  vec3 worldDir = viewWorld * ray;
  vec3 col;
  float d = rayMarch(eye, worldDir, col);

  vec3 v = eye + worldDir*d;

	if(d < MaxDist){
		// float visibleToLight = shadowMarch(eye + ray * d, lightPos);
		// float visibleToLight = shadowMarch(eye+worldDir*(d-0.001), lightPos);
	
		vec3 n = estimateNormal(v);
		float lights = lighting(v, n, lightPos);

		i += lights;// * col.x;	

		float fog = 1./pow(d, 1.1);
		i *= fog;
	}

	gl_FragColor = vec4(vec3(i), 1);
}
