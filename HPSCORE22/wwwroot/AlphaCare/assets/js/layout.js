/* Alpha Care – shared layout behaviour (include on every page)
   - Sidebar collapse: desktop slides out & content expands (remembered); tablet/mobile uses an overlay drawer
   - Optional: period toggle (#periodToggle) and refresh button (#refresh) – safely skipped if not on the page */
(function () {
  'use strict';
  var body = document.body;
  var sidebar = document.getElementById('sidebar');
  var mq = window.matchMedia('(max-width:1024px)');
  var KEY = 'sidebarCollapsed';

  if (sidebar) {
    try { if (!mq.matches && localStorage.getItem(KEY) === '1') body.classList.add('sb-collapsed'); } catch (e) {}

    var closeDrawer = function () { sidebar.classList.remove('open'); body.classList.remove('sb-open'); };
    var toggle = function () {
      if (mq.matches) {
        var open = !sidebar.classList.contains('open');
        sidebar.classList.toggle('open', open);
        body.classList.toggle('sb-open', open);
      } else {
        var c = body.classList.toggle('sb-collapsed');
        try { localStorage.setItem(KEY, c ? '1' : '0'); } catch (e) {}
      }
    };

    var menuBtn = document.getElementById('menuBtn');
    var sbClose = document.getElementById('sbClose');
    var overlay = document.getElementById('overlay');
    if (menuBtn) menuBtn.addEventListener('click', toggle);
    if (sbClose) sbClose.addEventListener('click', toggle);
    if (overlay) overlay.addEventListener('click', closeDrawer);
    sidebar.querySelectorAll('.nav a').forEach(function (a) {
      a.addEventListener('click', function () { if (mq.matches) closeDrawer(); });
    });
    document.addEventListener('keydown', function (e) { if (e.key === 'Escape' && mq.matches) closeDrawer(); });
    if (mq.addEventListener) mq.addEventListener('change', closeDrawer);
  }

  var pt = document.getElementById('periodToggle');
  if (pt) {
    pt.querySelectorAll('button').forEach(function (b) {
      b.addEventListener('click', function () {
        pt.querySelectorAll('button').forEach(function (x) { x.classList.remove('on'); });
        b.classList.add('on');
      });
    });
  }

  var rf = document.getElementById('refresh');
  if (rf && rf.firstElementChild) {
    rf.addEventListener('click', function () {
      var ic = rf.firstElementChild;
      ic.style.transition = 'transform .6s';
      ic.style.transform = 'rotate(360deg)';
      setTimeout(function () { ic.style.transform = 'none'; }, 700);
    });
  }
})();

/* Sidebar submenus (accordion groups). Open/closed state is remembered across pages;
   a group containing the current page's .sub-link.active is always opened. */
(function () {
  'use strict';
  var groups = document.querySelectorAll('.nav-group');
  if (!groups.length) return;
  var KEY = 'sidebarGroups', saved = {};
  try { saved = JSON.parse(localStorage.getItem(KEY) || '{}') || {}; } catch (e) {}

  function setOpen(g, open) {
    g.classList.toggle('open', open);
    var b = g.querySelector('.nav-toggle');
    if (b) b.setAttribute('aria-expanded', open ? 'true' : 'false');
  }

  Array.prototype.forEach.call(groups, function (g, i) {
    var id = g.getAttribute('data-group') || String(i);
    var btn = g.querySelector('.nav-toggle');
    var hasActive = !!g.querySelector('.sub-link.active');
    if (hasActive && btn) btn.classList.add('has-active');
    setOpen(g, hasActive || (id in saved ? !!saved[id] : g.classList.contains('open')));
    if (!btn) return;
    btn.addEventListener('click', function () {
      var open = !g.classList.contains('open');
      setOpen(g, open);
      saved[id] = open;
      try { localStorage.setItem(KEY, JSON.stringify(saved)); } catch (e) {}
    });
  });
})();
