// 64
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define TAU 2.*PI
#define NUM_STRIPS 10.

vec2 swap(vec2 p){
  return vec2(p.y,p.x);
}


void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  // p = swap(p);float t = u_time*0.;p *= .15;p.x += 1.0;
  
  // we know the x len and y len we also 
  // know the hypotenuse so we can calc z

  // We are working with a sphere, so the vectors along
  // the surface have length = 1. Since we know the x and y
  // on the canvas, we can derive the Z.
  // Now  each pixel now has a normal vector.
  vec3 n = vec3(p, sqrt(1.-p.x*p.x-p.y*p.y));

  // Let's focus on the vertical stripes. How can we do this?
  // Imagine if we cut the sphere in half horizontally.
  // The alternating stripes show us how the angles change.
  // If we mod on the angle, we can alternate the colors.
  
  // Remove the Y then normalize / Project onto XZ-plane
  // The result is a normal on the XZ-plane
  vec3 xzVec = normalize(vec3(n.x, 0., n.z));
  float thetaY = atan(xzVec.x, xzVec.z)+ u_time;

  float div = 1.;
  float pLen = length(p);

  // ew i know i know
  // float horizSlices = 1.;
  // float h = .25 * step(mod(p.y, horizSlices), horizSlices/2.);
  // float i = step(div/4., mod(thetaY + h,div/2.)) * step(length(p), 1.);

  // vertical stripes

  // why div 4?
  float i = step(div/4., mod(thetaY,div/2.));

  gl_FragColor = vec4(vec3(i),1.);
  // if(pLen > 1.){gl_FragColor = vec4(0,0,0,1);}
}

/*
More notes:

We 'could' get away without normalizing...But that also means 
the pixels 'beyond' the sphere will get colored incorrectly.

*/