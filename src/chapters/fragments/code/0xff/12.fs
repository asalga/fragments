precision mediump float;
uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define TAU (PI*2.)

float circleSDF(vec2 p, float r){return length(p)-r;}
float ringSDF(vec2 p, float r, float w){
  return abs(length(p)-r*0.5)-w;
}
mat2 r2d(float _a){
  return mat2(cos(_a),-sin(_a),sin(_a),cos(_a));
}

void main(){
  vec2 as = vec2(1., u_res.y/u_res.x);
  vec2 p = as * (gl_FragCoord.xy/u_res*2.-1.);

// float i = step(circleSDF(p, 1.), 0.);  
// everywehre where there is a white pixel
// we'll need to draw a ring, BUT we'll need
// to space out the rings
// also, well need to pass in different args to the 
// ringSDF

// we are calling ring for every pixel, but the problem
// is that it isn't getting satisfied (returnin true)
// so the question is, how can we make it return true?

  // p += vec2(0.5, 0.);
  
  vec2 a = vec2(1., 0.);
  vec2 b = normalize(p);

  float angleBetween = acos(dot(b,a));

  //float i = step(ringSDF(p + r , .54, 0.01), 0.0);
  float i = 0.;

  if(b.y < 0.){
    angleBetween = PI + (PI-angleBetween);
  }
  // angleBetween *= 1.-(length(p)*1.);
  // gl_FragColor.rgb += vec3(angleBetween/(PI*2.));

  // p -= (a*.5) * r2d(angleBetween * 3.);
  // p += (a) * r2d(angleBetween * 6.);

  // p += a * r2d(angleBetween*3.);

  // if(mod(angleBetween/PI, .0) < PI){
  	
  	// i += step(ringSDF(p, .13, .05), 0.01);
  	// i += ringSDF(p, .13, .05);

	// make it calculate the distance from 
	// not from the origin, but from 
	// 
  	// i += step(circleSDF(p, .81), 0.01);

	// if(mod(angleBetween/TAU, 0.1) < 1.){
	float normAngle = angleBetween/TAU;

	
	if(mod(normAngle*10.0, 2.) < 1. ){
	  p += a * r2d(angleBetween*3.);
	  i += step(ringSDF(p, .5, 0.01), 0.3);
	  gl_FragColor = vec4(vec3(0.5, i, i), 1.);	
	}
	else{
	  // gl_FragColor = vec4(angleBetween/TAU, i, 0., 1.);
	  gl_FragColor = vec4(vec3(i), 1.);
	}
}

  	

  	// i += circleSDF(p, .001);

  // gl_FragColor += vec4(vec3(i),1.);

  // reduce size of ring, offset
  // float i = step(ringSDF(p, 1., 0.01), 0.0);
  // step(ringSDF(p * atan(p.y,p.x), 1., 0.1), 0.01);
  // , 0.001
  // ) * 
  // ringSDF(p, 0.01, 0.01);

  



// if the current point is on the edge of the ring,
// put one there
// so how do we determine if point is on the edge?

// well, we have an sdf function already!
// what we have right now is we get the point then 
// draw a 'portion' of the ring


  // oKay, we can draw a ring.
  // how can we draw a series of rings so that
  // they make up another ring?

  // Why can't we just get the angle and put a
  // ring there?

  // So let's create a ring sdf and every time the angle
  // between is then draw a ring there.
// }

// float an = acos(dot(a,b));
  // if(b.y > 0.){
  	// an -= PI*2.;
  // }
	// an = u_mouse.y/u_res.y;
  // float an = acos(a*PI);
  // float t = atan(p1.y,p1.x) * u_time;
  // vec2 v1 = vec2(1., 0.);

  // an *= PI;
  // an += u_time;