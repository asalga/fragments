/*
    Andor Saga
*/

'use strict';

let YesMakeGif = false;
let gif;

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

  var sketch = function(p) {

    let renderers = {};
    let graphicsCtx = [];
    let progObjects = [];

    p.preload = function() {

      // same vertex shader for each frag shader
      let globalVs = `precision highp float;
                    varying vec2 vPos;
                    attribute vec3 aPosition;
                    void main() {
                      vPos = (gl_Position = vec4(aPosition,1.0)).xy;
                    }`;

      let mainFs = `precision mediump float;
                    uniform sampler2D lastBuffer;
                    void main(){
                      vec2 p = gl_FragCoord.xy/vec2(300.);
                      vec4 col = texture2D(lastBuffer, p);
                      gl_FragColor = vec4(col.rgb,1);
                    }`;
      
      mainShader = p.createShader(globalVs, mainFs);

      fs.forEach( _fs => {
        console.log('creating renderer', _fs);
        graphicsCtx.push(p.createGraphics(w, h, p.WEBGL));
        progObjects.push(p.createShader(globalVs,_fs));
      });

      // TODO: fix
      // if (params.tex0) {img0 = p.loadImage(params.tex0);}
      // if (params.tex1) {img1 = p.loadImage(params.tex1);}
      // if (params.tex2) {img2 = p.loadImage(params.tex2);}

      console.log('preload done');
    };

    p.setup = function() {
      w = params.width || DefaultSketchWidth;
      h = params.height || DefaultSketchHeight;
      sketchTime = 0;
      tracking = [0, 0];

      var c = p.createCanvas(w, h, p.WEBGL);
      p.pixelDensity(1);

      // reset anim
      c.mouseClicked(e=>{  start = p.millis();   });
      c.mousePressed(e=>{  mouseIsDown = 1;      });
      c.mouseReleased(e=>{
        mouseIsDown = 0;

        let x = p.mouseX.clamp(0, w);
        let y = p.mouseY.clamp(0, h);
        lastMouseDown = [x,y];
      });

      $(p.canvas).appendTo($('#target'));
      p.loop();
    };


    /**
          Draw
    */
    p.draw = function() {

      // CEL
      // gfx3D.push();  
      // gfx3D.translate(-width / 2, -height / 2);
      // gfx3D.shader(celShader);
      // celShader.setUniform('time', millis());
      // celShader.setUniform('numShades', controls.celShades);
      // celShader.setUniform('res', [width, height]);
      // celShader.setUniform('mouse', [pmouseX, height - pmouseY, mouse[0], mouse[2]]);
      // celShader.setUniform('texture0', gfx);
      // gfx3D.rect(0, 0, windowWidth, windowHeight, 1, 1);
      // gfx3D.pop();

      let x = 0;
      progObjects.forEach( (progObj, i) =>{        
        let ctx = graphicsCtx[i];
        ctx.shader(progObj);

        if(i !== 0){
          progObj.setUniform('buff', graphicsCtx[i-1]);
        }

        ctx.quad(-1, -1, 1, -1, 1, 1, -1, 1);
      });

      // final
      p.shader(mainShader);
      mainShader.setUniform('lastBuffer', graphicsCtx[progObjects.length-1]);
      p.quad(-1, -1, 1, -1, 1, 1, -1, 1);
      
      // p.image(0, 0, w, h, graphicsCtx[0]);

      // for tweening in animation
      // sketchTime += (1 / 60) * timeVal.t;
      // //sketchTime = p.millis() / 1000 * 0.5;

      // // for resetting animation
      // sketchTime = (p.millis()-start) / 1000 * 0.5;

      // if (fs.match(/uniform\s+vec2\s+u_res/)) {
      //   sh.setUniform('u_res', [w, h]);
      // }
      // if (fs.match(/uniform\s+float\s+u_time/)) {
      //   sh.setUniform('u_time', sketchTime);
      // }
      // if(fs.match(/uniform\s+vec2\s+u_tracking/)){
      //   // target - currPos
      //   let x = p.mouseX.clamp(0, w);
      //   let y = p.mouseY.clamp(0, h);

      //   let delta = [(x/w) - tracking[0],
      //                (y/h) - tracking[1]];
      //   tracking = [tracking[0] + delta[0] * easing,
      //               tracking[1] + delta[1] * easing];


      //   sh.setUniform('u_tracking', tracking);
      // }

      // // TODO: Add for loop here
      // if (fs.match(/uniform\s+sampler2D\s+u_texture0/)) {
      //   sh.setUniform('u_texture0', img0);
      // }

      // if (fs.match(/uniform\s+sampler2D\s+u_texture1/)) {
      //   sh.setUniform('u_texture1', img1);
      // }

      // if (fs.match(/uniform\s+sampler2D\s+u_texture2/)) {
      //   sh.setUniform('u_texture2', img2);
      // }


      // if (fs.match(/uniform\s+vec3\s+u_mouse/)) {
      //   let x = p.mouseX.clamp(0, w);
      //   let y = p.mouseY.clamp(0, h);
      //   sh.setUniform('u_mouse', [x, y, mouseIsDown]);
      // }

      // if (fs.match(/uniform\s+vec2\s+u_lastMouseDown/)) {
      //   sh.setUniform('u_lastMouseDown', lastMouseDown);
      // }
      // // sh.setUniform('u_lastMouseDown', lastMouseDown);

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
  },
  '2': {
    src: '../fragments/code/0xff/90/99/2.fs'
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

  //   .then(res => res.text())
  //     .then(fragShaderCode => {
  //       let relPath = '';
  //       let fragShaders = [fragShaderCode]
  //       let sketch = new p5(makeSketch(fragShaders,{}), relPath);
  //     });

})();