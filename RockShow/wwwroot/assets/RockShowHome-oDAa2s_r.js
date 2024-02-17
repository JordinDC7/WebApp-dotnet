import{j as s,r as Y,u as F}from"./index-h_YPr0qD.js";import{N as H,F as q}from"./NavBar-8uXd1MzC.js";var R={},P;Object.defineProperty(R,"__esModule",{value:!0});var C=s,l=Y,i=function(){return i=Object.assign||function(e){for(var r,o=1,t=arguments.length;o<t;o++)for(var n in r=arguments[o])Object.prototype.hasOwnProperty.call(r,n)&&(e[n]=r[n]);return e},i.apply(this,arguments)};function M(e,r){var o,t;switch(r.type){case"TYPE":return i(i({},e),{speed:r.speed,text:(o=r.payload)===null||o===void 0?void 0:o.substring(0,e.text.length+1)});case"DELAY":return i(i({},e),{speed:r.payload});case"DELETE":return i(i({},e),{speed:r.speed,text:(t=r.payload)===null||t===void 0?void 0:t.substring(0,e.text.length-1)});case"COUNT":return i(i({},e),{count:e.count+1});default:return e}}var O=function(e){var r=e.words,o=r===void 0?["Hello World!","This is","a simple Typewriter"]:r,t=e.loop,n=t===void 0?1:t,u=e.typeSpeed,a=u===void 0?80:u,v=e.deleteSpeed,E=v===void 0?50:v,h=e.delaySpeed,x=h===void 0?1500:h,d=e.onLoopDone,g=e.onType,p=e.onDelete,T=e.onDelay,S=l.useReducer(M,{speed:a,text:"",count:0}),b=S[0],_=b.speed,y=b.text,w=b.count,f=S[1],j=l.useRef(0),m=l.useRef(!1),c=l.useRef(!1),k=l.useRef(!1),D=l.useRef(!1),L=l.useCallback(function(){var N=w%o.length,B=o[N];c.current?(f({type:"DELETE",payload:B,speed:E}),y===""&&(c.current=!1,f({type:"COUNT"}))):(f({type:"TYPE",payload:B,speed:a}),k.current=!0,y===B&&(f({type:"DELAY",payload:x}),k.current=!1,D.current=!0,setTimeout(function(){D.current=!1,c.current=!0},x),n>0&&(j.current+=1,j.current/o.length===n&&(D.current=!1,m.current=!0)))),k.current&&g&&g(j.current),c.current&&p&&p(),D.current&&T&&T()},[w,x,E,n,a,o,y,g,p,T]);return l.useEffect(function(){var N=setTimeout(L,_);return m.current&&clearTimeout(N),function(){return clearTimeout(N)}},[L,_]),l.useEffect(function(){d&&m.current&&d()},[d]),[y,{isType:k.current,isDelay:D.current,isDelete:c.current,isDone:m.current}]},U="styles-module_blinkingCursor__yugAC",V="styles-module_blinking__9VXRT";(function(e,r){r===void 0&&(r={});var o=r.insertAt;if(e&&typeof document<"u"){var t=document.head||document.getElementsByTagName("head")[0],n=document.createElement("style");n.type="text/css",o==="top"&&t.firstChild?t.insertBefore(n,t.firstChild):t.appendChild(n),n.styleSheet?n.styleSheet.cssText=e:n.appendChild(document.createTextNode(e))}})(".styles-module_blinkingCursor__yugAC{color:inherit;font:inherit;left:3px;line-height:inherit;opacity:1;position:relative;top:0}.styles-module_blinking__9VXRT{animation-duration:.8s;animation-iteration-count:infinite;animation-name:styles-module_blink__rqfaf}@keyframes styles-module_blink__rqfaf{0%{opacity:1}to{opacity:0}}");var A=l.memo(function(e){var r=e.cursorBlinking,o=r===void 0||r,t=e.cursorStyle,n=t===void 0?"|":t,u=e.cursorColor,a=u===void 0?"inherit":u;return C.jsx("span",i({style:{color:a},className:"".concat(U," ").concat(o?V:"")},{children:n}))});R.Cursor=A,P=R.Typewriter=function(e){var r=e.words,o=r===void 0?["Hello World!","This is","a simple Typewriter"]:r,t=e.loop,n=t===void 0?1:t,u=e.typeSpeed,a=u===void 0?80:u,v=e.deleteSpeed,E=v===void 0?50:v,h=e.delaySpeed,x=h===void 0?1500:h,d=e.cursor,g=d!==void 0&&d,p=e.cursorStyle,T=p===void 0?"|":p,S=e.cursorColor,b=S===void 0?"inherit":S,_=e.cursorBlinking,y=_===void 0||_,w=e.onLoopDone,f=e.onType,j=e.onDelay,m=e.onDelete,c=O({words:o,loop:n,typeSpeed:a,deleteSpeed:E,delaySpeed:x,onLoopDone:w,onType:f,onDelay:j,onDelete:m})[0];return C.jsxs(C.Fragment,{children:[C.jsx("span",{children:c}),g&&C.jsx(A,{cursorStyle:T,cursorColor:b,cursorBlinking:y})]})},R.useTypewriter=O;function G(){const e=F(),r=()=>{e("/RockShop")};return s.jsxs("div",{className:"container top-section",children:[s.jsx(H,{}),s.jsx("div",{className:"min-h-screen flex items-center justify-center text-center",children:s.jsxs("div",{children:[s.jsxs("h1",{className:"text-4xl font-bold text-gray-900",children:["Discover OracleIllusions"," ",s.jsx("span",{className:"text-purple-600",children:s.jsx(P,{words:["Rocks","Gems","& More"],loop:!1,cursor:!0,cursorStyle:"|",typeSpeed:110,deleteSpeed:110,delaySpeed:1e3})})]}),s.jsx("p",{className:"mt-4 text-lg text-white-50",children:"Step into the world of pristine natural beauty. Shop our collection and find your perfect gem today."}),s.jsx("button",{onClick:r,className:"mt-6 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-lg text-lg",children:"Browse Shop"})]})}),s.jsx(q,{})]})}export{G as default};