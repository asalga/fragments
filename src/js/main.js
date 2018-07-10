/*
    Andor Saga
*/
'use strict';

let shader_1_Frag;
let postProcessFrag;
let globalVs = `
precision highp float;
attribute vec3 aPosition;

uniform mat4 uModelViewMatrix;
uniform mat4 uProjectionMatrix;

void main() {
  gl_Position = uProjectionMatrix * uModelViewMatrix * vec4(aPosition, 1.0 );
}`;



let gif, YesMakeGif = false;

Number.prototype.clamp = function(min, max) {
  return Math.min(Math.max(this, min), max);
};

function animate(time) {
  requestAnimationFrame(animate);
  // TWEEN.update(time);
}
requestAnimationFrame(animate);


/*
  fs - array of frag shaders
*/
function makeSketch(fs, params) {
  const DefaultSketchWidth = 320;
  const DefaultSketchHeight = 240;
  
  let mainShader;
  let w, h;
  let img0, img1, img2;

  let timeVal = { t: 0 };
  let sketchTime;
  let tracking = [];
  let easing = 0.05;
  let start = 0;
  let mouseIsDown = 0;
  let lastMouseDown= [120,200];

  let gfx, gfx3D;
  let shader_1_, postProcessShader;

  var sketch = function(p) {

    let renderers = {};
    let graphicsCtx = [];
    let progObjects = [];

    p.preload = function() {

      let mainFs = `precision mediump float;
                    uniform sampler2D lastBuffer;
                    void main(){
                      vec2 p = gl_FragCoord.xy/vec2(300.);
                      vec4 col = texture2D(lastBuffer, vec2(0.5));
                      gl_FragColor = vec4(col.rg,1, 1);
                    }`;

      let fs1 =    `precision mediump float;
                    void main(){
                      vec2 p = gl_FragCoord.xy/vec2(300.);
                      gl_FragColor = vec4(1, 1, 0, 1);
                    }`;
      
      mainShader = p.createShader(globalVs, mainFs);
      //new p5.Shader(p._renderer, globalVs, mainFs);

      gfx3D = p.createGraphics(w,h, p.WEBGL);
      
      shader_1_Frag = fs[0];
      postProcessFrag = fs[1];

      fs.forEach( _fs => {
        //   console.log('creating renderer', _fs);
        //   let ctx = p.createGraphics(w, h, p.WEBGL);
        //   graphicsCtx.push(ctx);
          
        //   let newShader = new p5.Shader(ctx._renderer, globalVs, _fs);
        //   progObjects.push(newShader);

        // progObjects.push(p.createShader(ctx, globalVs, _fs));
      });

      // TODO: fix
      // if (params.tex0) {img0 = p.loadImage(params.tex0);}
      // if (params.tex1) {img1 = p.loadImage(params.tex1);}
      // if (params.tex2) {img2 = p.loadImage(params.tex2);}
    };

    p.setup = function() {
      w = params.width || DefaultSketchWidth;
      h = params.height || DefaultSketchHeight;
      sketchTime = 0;
      tracking = [0, 0];

      p.createCanvas(p.windowWidth, p.windowHeight, p.WEBGL);
      
      gfx = p.createGraphics(p.windowWidth, p.windowHeight, p.WEBGL);
      gfx3D = p.createGraphics(p.windowWidth, p.windowHeight, p.WEBGL);

      shader_1_ = new p5.Shader(gfx3D._renderer, globalVs, shader_1_Frag);
      postProcessShader = new p5.Shader(p._renderer, globalVs, postProcessFrag);
      
      // var c = p.createCanvas(w, h, p.WEBGL);
      p.pixelDensity(1);

      // reset anim
      // c.mouseClicked(e=>{  start = p.millis();   });
      // c.mousePressed(e=>{  mouseIsDown = 1;      });
      // c.mouseReleased(e=>{
      //   mouseIsDown = 0;
      //   let x = p.mouseX.clamp(0, w);
      //   let y = p.mouseY.clamp(0, h);
      //   lastMouseDown = [x,y];
      // });

      $(p.canvas).appendTo($('#target'));
      p.loop();
    };


    /**
          Draw
    */
    p.draw = function() {
      let width = p.windowWidth;
      let height = p.windowHeight;

      gfx.push();
      // gfx.translate(-width/2, -height/2);
      // gfx.translate(-width / 2, -height / 2);
      gfx.background(0,255,0);
      gfx.fill(255,0,0);
      gfx.stroke(0,245,0);
      gfx.rect(0,0,1901,1091);
      gfx.sphere(140);
      gfx.pop();


      // CEL
      gfx3D.push();  
      // gfx3D.translate(-width / 2, -height / 2);
      gfx3D.shader(shader_1_);
      shader_1_.setUniform('res', [width, height]);
      shader_1_.setUniform('t0', gfx);
      gfx3D.rect(-width, -height, width, height, 1, 1);
      gfx3D.sphere(1020);
      // gfx3D.quad(-1, -1, 1, -1, 1, 1, -1, 1);
      gfx3D.pop();
      
      p.push();
      p.translate(width / 2, height / 2);
      p.shader(postProcessShader);
      postProcessShader.setUniform('res', [width, height]);
      postProcessShader.setUniform('t0', gfx);
      // p.rect(0, 0, p.windowWidth, p.windowHeight, 1, 1);
      p.strokeWeight(3);
      p.stroke(0);
      let sz = 13;
      p.rect(-width*sz, -height*sz, width*sz, height*sz, 3, 3);
      p.quad(-2, -2, 2, -2, 2, 2, -2, 2);
      p.sphere(10);
      p.pop();

    };//end draw
  };
  return sketch;
}

let demo = {
  '0': {
    src: '../fragments/code/0xff/90/99/0.fs'
  },
  '1': {
    src: '../fragments/code/0xff/90/99/1.fs'
  }
};


function getFs0(){
  return fetch('../fragments/code/0xff/90/99/0.fs')
    .then(res => res.text())
    .then(fragShaderCode => {
      return fragShaderCode;
    });
}
function getFs1(){
  return fetch('../fragments/code/0xff/90/99/1.fs')
    .then(res => res.text())
    .then(fragShaderCode => {
      return fragShaderCode;
    });
}

(function load(){
  Promise.all([getFs0(), getFs1()])
    .then(fragShaders => {
      let relPath;
      let sketch = new p5(makeSketch(fragShaders,{}), relPath);
    });
})();