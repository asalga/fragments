// 64
precision mediump float;
uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define TAU 2.*PI
#define DEG_TO_RAD 180./PI
#define FOV 65.

#define NUM_VERT_STRIPS 5.
#define NUM_HORIZ_STRIPS 5.

vec2 swap(vec2 p){
  return vec2(p.y,p.x);
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  vec3 upVec = vec3(0., 1., 0.);
  float i;
  // p = swap(p);float t = u_time*0.;p *= .15;p.x += 1.0;
  
  // We want to apply some foreshortening to the scene,

  // --- The FOV determines the angle the rays shoot 
  // into the scene
  // Calculate the base of the viewing triangle so that we know how far 
  // the rays span. 
  // float camRatio = tan( (FOV/2.) * DEG_TO_RAD);
  // p.y *= camRatio;

  // vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
  //   vec2 xy = fragCoord - size / 2.0;
  //   float z = size.y / tan(radians(fieldOfView) / 2.0);
  //   return normalize(vec3(xy, -z));
  // }

  // rayDirection(45.0, iResolution.xy, fragCoord);

  // we know the x len and y len we also 
  // know the hypotenuse so we can calc z

  // We are working with a sphere, so the vectors along
  // the surface have length = 1. Since we know the X and Y
  // on the canvas, we can derive the Z.
  // Now  each pixel now has a normal vector.
  vec3 n = normalize(vec3(p, sqrt(1.-p.x*p.x-p.y*p.y)));

  // Let's focus on the vertical stripes. How can we do this?
  // Imagine if we cut the sphere in half horizontally.
  // The alternating stripes show us how the angles change.
  // If we mod on the angle, we can alternate the colors.

  // Longitude  
  // Remove the Y then normalize / Project onto XZ-plane
  // The result is a normal on the XZ-plane
  vec3 xzVec = normalize(vec3(n.x, 0., n.z));
  float thetaY = atan(xzVec.x,xzVec.z);// -PI/2 to PI/2
  thetaY = (thetaY*2.)/PI;// -1 .. 1
  thetaY = (thetaY+1.)/2.;// 0 .. 1
  // thetaY += u_time/4.;// animate

  vec3 yzVec = normalize(vec3(n.x, n.y, 0.));
  float thetaX = atan(yzVec.y, yzVec.y);// -PI/2 to PI/2
  thetaX = (thetaX*2.)/PI;// -1 .. 1
  thetaX = (thetaX+1.)/2.;// 0 .. 1
  


  vec3 zVec = (vec3(n.x, 0., n.z));
  float test = dot(zVec, upVec);
  // float test = atan(zVec.y, zVec.y);// -PI/2 to PI/2
  // test = (test*2.)/PI;// -1 .. 1
  // test = (test+1.)/2.;// 0 .. 1



  n.y *= dot(n, upVec);
  n = normalize(n);

  // n.y = pow(n.y, 1.1);
  // n.y -=  0.01 * abs(5.*n.x)/(n.y);
  // n = normalize(n);
  // Latitude
  // compare normal with up vector
  // n.y *= abs(test)*1.;
  // n.y += sin(p.x/TAU)/10.;
  // n=normalize(n);
  float latTheta = dot(n, vec3(0., 1., 0.));

  // i = mod(thetaX*test, 0.2);
  // float i = step(mod(thetaY, 1./NUM_VERT_STRIPS), 0.24/2.);
  //mod(thetaY, 1./NUM_VERT_STRIPS);

  //float horizSlices = .1; 
  // float h = .25 * step(mod(p.y, horizSlices), horizSlices/2.);  

  // mod 0..1 into how many stripes we want
  float lon = mod(thetaY, 1./NUM_VERT_STRIPS) * NUM_VERT_STRIPS;
  float lat = mod(latTheta, .5) * 2.;
  // float lat = //mod(thetaX, 1./NUM_VERT_STRIPS) * NUM_VERT_STRIPS;

  i = lat;


  // i = step(m,.5) * step(length(p), 1.);
  // i = step(lon, .5);  // .5 since there are 2 states
  // i = step(lat, .5);
  
  // float h = .25 * step(mod(p.y, horizSlices), horizSlices/2.);
  // float i = step(div/4., mod(thetaY + h,div/2.)) * step(length(p), 1.);

  // float div = 1.;
  // float horizSlices = 0.4;
  // float h = .25 * step(mod(p.y, horizSlices), horizSlices/2.);
  // float i = step(div/4., mod(thetaY + h,div/2.)) * step(length(p), 1.0);
  gl_FragColor = vec4(vec3(i),1.);
}

/*
More notes:

We 'could' get away without normalizing...But that also means 
the pixels 'beyond' the sphere will get colored incorrectly.

// float i = smoothstep(0.50, 0.49, m) - smoothstep(0.01, 0.001, m);
*/