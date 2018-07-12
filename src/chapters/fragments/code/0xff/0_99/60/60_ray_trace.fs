// 6x - "ray trace" method from scratchapixel
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define TAU 2.*PI
#define RAD 0.5
#define CNT 3
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = 1.5 * (a * (gl_FragCoord.xy/u_res*2.-1.));
  vec3 col;
  for(int i = 0; i < CNT; i++){
    float f_it = TAU * (float(i)/float(CNT));
    float t = f_it + u_time*2.;
    // no perspective, changing distance = no foreshortening
    vec3 spherePos = vec3(cos(t), sin(t), -2.);
    vec3 rayOrigin = vec3(p.x, p.y, 0.);
    vec3 rayDir = vec3(0., 0, -1.);
    vec3 rayToSphere = spherePos - rayOrigin;
    // Project vector rayToSphere onto rayDir
    float tca = dot(rayToSphere, rayDir);
    if(tca < 0.){discard;}// if -ve, no intersection
    // d = sqrt(rayToSphere² - tca²)
    float d =sqrt(dot(rayToSphere,rayToSphere)-dot(tca,tca));
    // thc = sqrt(r² - d²)
    float thc = sqrt(pow(RAD,2.)-pow(d,2.));
    float t0 = tca - thc; // how much to scale ray for hit
    float c = 2.*clamp(2. - t0, 0., 1.);
    col += vec3(c);// basic shading
  }
  gl_FragColor = vec4(col,1.);
}