precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658

float sdSquare(vec2 p, float halfW){
  vec2 _p = abs(p);
  return max(_p.x,_p.y)-halfW;
}

float every(float v, float i, float c, float stay){
	float alternate = step(mod(v, c), i);
	float x = mod(v, i) * alternate;
	// we reached the 'end'
	if(stay == 1. && alternate==0.){x = i;}
	return x;
}


// from iquilezles 
float impulse(float k, float x){
  float h = k*x;
  return h*exp(1.0-h);
}

mat2 r2d(float a){
	return mat2(cos(a),-sin(a),sin(a), cos(a));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time * 1.0;
  const float cnt = 4.;

  float mvX = every(t     , 1., 2., 1.);
  float mvY = every(t + 1., 1., 2., 0.);

  float flip = step(mod(t, 4.),2.)*2.-1.;
  p *= flip;
	float sz = .25;

  
  for(float it=0.;it < cnt;it++){
  	float pct = (it/(cnt));
  
	  mvX = impulse(1., mvX);
	  mvY = impulse(1., mvY);

	  vec2 pos = vec2(.5-mvX, -.5+mvY);

  
  	float alpha = 0.25;
  	i += step(sdSquare(p+pos, sz),0.) * alpha;
	}

  gl_FragColor = vec4(vec3(i),1.);
}




















