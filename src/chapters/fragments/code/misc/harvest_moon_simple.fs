precision mediump float;
uniform vec2 u_res;uniform float u_time;
#define PI 3.1415926
#define V2 vec2
float r(vec2 p, float r, float w){
return abs(length(p)-r*.5)-w;}
void main(){
V2 a=V2(1.,u_res.y/u_res.x);
V2 p=1.1*a*(gl_FragCoord.xy/u_res*2.-1.);
float t=u_time*2.*PI/2.- PI/2.;
vec3 l=vec3(cos(t),0.,sin(t));
float z=sqrt(1.-p.x*p.x-p.y*p.y);
vec3 v=normalize(vec3(p.x,p.y,z));
float c=step(dot(v,l),0.);
c += smoothstep(0.01,0.001,r(p,2.,.0004));
gl_FragColor = vec4(vec3(c),1.);}