/* Dashboard page only – data, charts and tables (needs layout markup ids: prob, donut, chart, techBody, shopBody) */
/* ---------- DATA ---------- */
const problems = [
  {name:'Air Conditioner (AC) Issues',cur:248,prev:205},
  {name:'Chiller Plant / Cold Storage',cur:176,prev:158},
  {name:'Electric Line & Power Circuit',cur:69,prev:72},
  {name:'Stand Fridger',cur:22,prev:20},
  {name:'Momo Steamer & Kitchen Machinery',cur:18,prev:14},
  {name:'Network LAN / Access Terminal',cur:62,prev:49},
];
const techs = [
  {n:'Forhad',d:'AC Division - Dhaka',id:'599668',c:59,p:53,g:'+6',r:'11.32%',pos:true},
  {n:'Hafizul',d:'AC Division - Dhaka',id:'584205',c:57,p:40,g:'+17',r:'42.50%',pos:true},
  {n:'Rasel-2 Jr.',d:'Dhaka North Hub',id:'386499',c:40,p:45,g:'-5',r:'-11.11%',pos:false},
  {n:'Emon',d:'AC Division - Dhaka',id:'587817',c:34,p:27,g:'+7',r:'25.93%',pos:true},
  {n:'Shoriful',d:'CTG Regional Hub',id:'406980',c:23,p:36,g:'-13',r:'-36.11%',pos:false},
];
const shops = [
  {code:'35830',name:'TT-EAST NASIRABAD CTG',t:9,s:'Critical'},
  {code:'36293',name:'TT-Akshay Tower-100 Feet',t:8,s:'Pending'},
  {code:'35982',name:'TT-LALKHAN MORE CTG',t:8,s:'Pending'},
  {code:'35545',name:'TT-MANIKNAGAR',t:8,s:'Resolved'},
  {code:'35590',name:'TT-BARIDHARA',t:8,s:'Resolved'},
];
const complain=[27,26,25,24,23,24,25,26,25,22,16,9,2,10,16,21,23,24,27,30,28,26,25,25,24,25,27,28,27,5];
const solved  =[25,25,24,22,21,22,23,24,22,20,15,8,1,8,13,17,19,20,22,25,22,19,18,18,17,17,18,18,16,1];
const startDate = new Date('2026-09-28');

/* ---------- PROBLEM BARS ---------- */
(function(){
  const max = Math.max(...problems.map(p=>Math.max(p.cur,p.prev)));
  document.getElementById('prob').innerHTML = problems.map(p=>`
    <div>
      <div class="r1"><span>${p.name}</span><span><b>${p.cur}</b> <em>(Prev: ${p.prev})</em></span></div>
      <div class="track"><div class="prev" style="width:${p.prev/max*100}%"></div><div class="cur" style="width:${p.cur/max*100}%"></div></div>
    </div>`).join('');
})();

/* ---------- DONUT ---------- */
(function(){
  const r=62,C=2*Math.PI*r,cx=80,cy=80,sw=22;
  const segs=[{v:70.4,c:'#10b981'},{v:20.0,c:'#f59e0b'},{v:9.6,c:'#ef4444'}];
  let off=0,out=`<circle cx="${cx}" cy="${cy}" r="${r}" fill="none" stroke="#f1f5f9" stroke-width="${sw}"/>`;
  segs.forEach(s=>{
    const len=C*s.v/100;
    out+=`<circle cx="${cx}" cy="${cy}" r="${r}" fill="none" stroke="${s.c}" stroke-width="${sw}"
      stroke-dasharray="${len-2} ${C-len+2}" stroke-dashoffset="${-off}" transform="rotate(-90 ${cx} ${cy})"/>`;
    off+=len;
  });
  document.getElementById('donut').innerHTML=out;
})();

/* ---------- LINE CHART ---------- */
(function(){
  const svg=document.getElementById('chart');
  const W=960,H=300,L=40,R=10,T=20,B=40,max=40;
  const iw=W-L-R, ih=H-T-B, n=complain.length;
  const x=i=>L+i*iw/(n-1), y=v=>T+ih-(v/max)*ih;
  const path=a=>{
    let d=`M${x(0)},${y(a[0])}`;
    for(let i=0;i<n-1;i++){
      const p0=[x(i-1<0?0:i-1),y(a[i-1<0?0:i-1])],p1=[x(i),y(a[i])],p2=[x(i+1),y(a[i+1])],p3=[x(i+2>n-1?n-1:i+2),y(a[i+2>n-1?n-1:i+2])];
      const c1=[p1[0]+(p2[0]-p0[0])/6,p1[1]+(p2[1]-p0[1])/6],c2=[p2[0]-(p3[0]-p1[0])/6,p2[1]-(p3[1]-p1[1])/6];
      d+=` C${c1[0]},${c1[1]} ${c2[0]},${c2[1]} ${p2[0]},${p2[1]}`;
    }
    return d;
  };
  let s='';
  [0,10,20,30,40].forEach(v=>{
    s+=`<line x1="${L}" x2="${W-R}" y1="${y(v)}" y2="${y(v)}" stroke="#e2e8f0" stroke-dasharray="${v?'3 4':'0'}"/>
        <text x="${L-12}" y="${y(v)+4}" text-anchor="end" font-size="11" fill="#64748b">${v}</text>`;
  });
  const labelIdx=[0,4,8,12,16,20,24,29];
  labelIdx.forEach(i=>{
    const d=new Date(startDate);d.setDate(d.getDate()+i);
    const t=d.toISOString().slice(0,10)+(i===29?' (Today)':'');
    s+=`<text x="${x(i)}" y="${H-12}" text-anchor="${i===29?'end':'middle'}" font-size="11" fill="#64748b">${t}</text>`;
  });
  s+=`<path d="${path(solved)}" fill="none" stroke="#10b981" stroke-width="2.5" stroke-linecap="round"/>`;
  s+=`<path d="${path(complain)}" fill="none" stroke="#3b82f6" stroke-width="2.5" stroke-linecap="round"/>`;
  const pts=(a,c)=>labelIdx.map(i=>`<circle cx="${x(i)}" cy="${y(a[i])}" r="4" fill="#fff" stroke="${c}" stroke-width="2"><title>${a[i]}</title></circle>`).join('');
  s+=pts(solved,'#10b981')+pts(complain,'#2563eb');
  svg.innerHTML=s;
})();

/* ---------- TABLES ---------- */
document.getElementById('techBody').innerHTML = techs.map((t,i)=>`
  <tr>
    <td style="color:#94a3b8">${i+1}</td>
    <td><b>${t.n}</b><small>${t.d}</small></td>
    <td class="mono">${t.id}</td>
    <td class="num"><b>${t.c}</b></td>
    <td class="num" style="color:#94a3b8">${t.p}</td>
    <td class="num ${t.pos?'pos':'neg'}">${t.g}</td>
    <td class="num"><span class="ratio ${t.pos?'r-g':'r-r'}">${t.r}</span></td>
  </tr>`).join('');

const stCls={Critical:'st-crit',Pending:'st-pend',Resolved:'st-res'};
document.getElementById('shopBody').innerHTML = shops.map((s,i)=>`
  <tr>
    <td style="color:#94a3b8">${i+1}</td>
    <td class="mono">${s.code}</td>
    <td style="font-weight:500;max-width:150px">${s.name}</td>
    <td class="num"><b>${s.t}</b></td>
    <td class="num"><span class="st ${stCls[s.s]}">${s.s}</span></td>
  </tr>`).join('');
