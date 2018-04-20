precision lowp float;
uniform vec2 u_res;
#define SS smoothstep
void main(){vec2 p=gl_FragCoord.xy/
u_res*2.-1.;vec2 s=vec2(cos(9.),sin(9.));
float z=sqrt(1.-p.x*p.x-p.y*p.y);
float len=length(vec3(p.x,p.y,z));
vec2 n=vec2(p.x,z)/len;
vec3 i=vec3(SS(.1,.13,dot(n,s))+ 
SS(.48,.5,1.-(length(p)-.5))-
SS(.38,.39,1.-(length(p)-.37)));
gl_FragColor=vec4(i,1.);}