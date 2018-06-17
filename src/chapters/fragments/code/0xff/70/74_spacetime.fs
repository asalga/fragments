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
  // float t = u_time * 2.;
  const float cnt = 10.;
	float sz = .25;

  vec2 border = p;
  i += step(0.9, border.x) + step(0.9, border.y);
  border *= -1.;
  i += step(0.9, border.x) + step(0.9, border.y);

	for(int sqIt=0;sqIt<2;sqIt++){
		float t = u_time * 2.;

	  float _sqIt = float(sqIt);
	  t += _sqIt*.5;

	  float flip = step(mod(t, 4.),2.)*2.-1.;
	  if(sqIt == 0){
			p *= flip;
		}

	  float mvX = every(t     , 1., 2., 1.);
    float mvY = every(t + 1., 1., 2., 0.);

	  for(float it=0.;it < cnt;it++){
	  	// accum impulse  
		  mvX = impulse(2., mvX*0.5);
		  mvY = impulse(2., mvY*0.5);

		  vec2 pos = vec2(.5-mvX, -.5+mvY);
	  	float alpha = 1./cnt;
	  	vec2 finalTrans = p+pos;
	  	finalTrans *= r2d( (mvX+mvY) *PI);
	  	i += smoothstep(0.01, 0.001, sdSquare( finalTrans , sz)) * alpha;
		}
	}
	
  gl_FragColor = vec4(vec3(i),1.);
}