/**
 * UBYS — StudentManagementSystem
 * site.js | Global JS Modülü
 *
 * Bağımlılıklar: jQuery 3.7+, SweetAlert2, DataTables
 */

"use strict";

const SMS = (() => {
    // ─── PRIVATE ────────────────────────────────────────────────────────────

    const _cfg = {
        dateFormat:  "DD.MM.YYYY",
        timeFormat:  "HH:mm",
        tablePageLen: 20,
        animDuration: 200,
        toastTimer:  3500,
    };

    // ─── INIT ────────────────────────────────────────────────────────────────

    function _init() {
        _initTheme();
        _initSidebar();
        _initTopbarToggle();
        _initSweetAlertMessages();
        _initConfirmForms();
        _initTooltips();
        _initTableSearch();
        _initDateDisplay();
        _autoHideAlerts();
    }

    // ─── TEMA ────────────────────────────────────────────────────────────────

    function _initTheme() {
        const role = document.body.dataset.role;
        if (!role) return;

        const map = {
            admin:         "theme-admin",
            ogretmen:      "theme-ogretmen",
            ogrenci:       "theme-ogrenci",
            ogrenciisleri: "theme-ogrenciisleri",
        };

        const cls = map[role.toLowerCase()];
        if (cls) document.documentElement.classList.add(cls);
    }

    // ─── SIDEBAR ─────────────────────────────────────────────────────────────

    function _initSidebar() {
        // Aktif linki belirle
        const currentPath = window.location.pathname.toLowerCase();
        document.querySelectorAll(".sidebar-link").forEach(link => {
            const href = link.getAttribute("href");
            if (!href) return;
            const linkPath = href.toLowerCase();
            if (currentPath === linkPath ||
                (linkPath !== "/" && currentPath.startsWith(linkPath))) {
                link.classList.add("active");
            }
        });

        // Mobil overlay
        const overlay = document.getElementById("sidebarOverlay");
        if (overlay) {
            overlay.addEventListener("click", () => SMS.sidebar.close());
        }
    }

    function _initTopbarToggle() {
        const toggleBtn = document.getElementById("sidebarToggle");
        if (toggleBtn) {
            toggleBtn.addEventListener("click", () => SMS.sidebar.toggle());
        }
    }

    // ─── SWEETALERT MESAJLAR ─────────────────────────────────────────────────

    function _initSweetAlertMessages() {
        const el = document.getElementById("swal-data");
        if (!el || typeof Swal === "undefined") return;

        const data = {
            success: el.dataset.success,
            error:   el.dataset.error,
            warning: el.dataset.warning,
            info:    el.dataset.info,
        };

        const map = {
            success: { icon: "success", timer: _cfg.toastTimer },
            error:   { icon: "error",   timer: _cfg.toastTimer + 1000 },
            warning: { icon: "warning", timer: _cfg.toastTimer },
            info:    { icon: "info",    timer: _cfg.toastTimer },
        };

        for (const [type, cfg] of Object.entries(map)) {
            if (data[type]) {
                SMS.toast(data[type], type);
            }
        }
    }

    // ─── CONFIRM FORMS ───────────────────────────────────────────────────────

    function _initConfirmForms() {
        document.querySelectorAll("[data-confirm]").forEach(el => {
            el.addEventListener("click", function (e) {
                e.preventDefault();
                const message = this.dataset.confirm || "Bu işlemi onaylıyor musunuz?";
                const title   = this.dataset.confirmTitle || "Emin misiniz?";
                const btnText = this.dataset.confirmBtn   || "Evet, devam et";
                const type    = this.dataset.confirmType  || "warning";
                const target  = this;

                SMS.confirm(title, message, btnText, type).then(result => {
                    if (!result.isConfirmed) return;

                    // Form içindeyse submit, link ise yönlendir
                    const form = target.closest("form");
                    if (form) {
                        form.submit();
                    } else if (target.href) {
                        window.location.href = target.href;
                    }
                });
            });
        });
    }

    // ─── TOOLTIP ─────────────────────────────────────────────────────────────

    function _initTooltips() {
        if (typeof bootstrap !== "undefined") {
            document.querySelectorAll("[data-bs-toggle='tooltip']").forEach(el => {
                new bootstrap.Tooltip(el, { trigger: "hover" });
            });
        }
    }

    // ─── TABLE SEARCH ────────────────────────────────────────────────────────

    function _initTableSearch() {
        document.querySelectorAll(".table-search input").forEach(input => {
            input.addEventListener("input", function () {
                const val = this.value.toLowerCase();
                const tableId = this.dataset.table;
                const rows = document.querySelectorAll(
                    tableId ? `#${tableId} tbody tr` : ".ubys-table tbody tr"
                );

                rows.forEach(row => {
                    const text = row.textContent.toLowerCase();
                    row.style.display = text.includes(val) ? "" : "none";
                });
            });
        });
    }

    // ─── TARİH DISPLAY ───────────────────────────────────────────────────────

    function _initDateDisplay() {
        document.querySelectorAll("[data-date]").forEach(el => {
            const raw = el.dataset.date;
            if (!raw) return;
            try {
                const d = new Date(raw);
                el.textContent = d.toLocaleDateString("tr-TR", {
                    day:   "2-digit",
                    month: "2-digit",
                    year:  "numeric",
                });
            } catch (_) {}
        });

        document.querySelectorAll("[data-datetime]").forEach(el => {
            const raw = el.dataset.datetime;
            if (!raw) return;
            try {
                const d = new Date(raw);
                el.textContent = d.toLocaleString("tr-TR", {
                    day: "2-digit", month: "2-digit", year: "numeric",
                    hour: "2-digit", minute: "2-digit",
                });
            } catch (_) {}
        });
    }

    // ─── AUTO HIDE ALERTS ────────────────────────────────────────────────────

    function _autoHideAlerts() {
        document.querySelectorAll(".alert[data-auto-dismiss]").forEach(alert => {
            const delay = parseInt(alert.dataset.autoDismiss, 10) || 4000;
            setTimeout(() => {
                alert.style.transition = "opacity .4s";
                alert.style.opacity = "0";
                setTimeout(() => alert.remove(), 400);
            }, delay);
        });
    }

    // ─── PUBLIC API ──────────────────────────────────────────────────────────

    return {
        init: _init,

        // ── Sidebar ──────────────────────────────────────────────────────
        sidebar: {
            toggle() {
                document.getElementById("appSidebar")?.classList.toggle("open");
                document.getElementById("sidebarOverlay")?.classList.toggle("show");
            },
            open() {
                document.getElementById("appSidebar")?.classList.add("open");
                document.getElementById("sidebarOverlay")?.classList.add("show");
            },
            close() {
                document.getElementById("appSidebar")?.classList.remove("open");
                document.getElementById("sidebarOverlay")?.classList.remove("show");
            },
        },

        // ── Toast ────────────────────────────────────────────────────────
        toast(message, type = "success") {
            if (typeof Swal === "undefined") { alert(message); return; }

            const iconMap = {
                success: "#10b981", error: "#ef4444",
                warning: "#f59e0b", info:  "#3b82f6",
            };

            Swal.fire({
                toast:             true,
                position:          "top-end",
                icon:              type,
                title:             message,
                showConfirmButton: false,
                timer:             _cfg.toastTimer,
                timerProgressBar:  true,
                iconColor:         iconMap[type] || iconMap.success,
                customClass: {
                    popup: "swal-toast-popup",
                    title: "swal-toast-title",
                },
            });
        },

        // ── Confirm ──────────────────────────────────────────────────────
        confirm(title, text, confirmText = "Evet", type = "warning") {
            if (typeof Swal === "undefined") {
                return Promise.resolve({ isConfirmed: window.confirm(text) });
            }

            const colorMap = {
                warning: "#f59e0b",
                danger:  "#ef4444",
                info:    "#3b82f6",
                success: "#10b981",
            };

            return Swal.fire({
                title,
                text,
                icon:              type === "danger" ? "error" : type,
                iconColor:         colorMap[type] || colorMap.warning,
                showCancelButton:  true,
                confirmButtonText: confirmText,
                cancelButtonText:  "İptal",
                confirmButtonColor: colorMap[type] || colorMap.warning,
                cancelButtonColor: "#94a3b8",
                focusCancel:       true,
                reverseButtons:    true,
            });
        },

        // ── DataTable ────────────────────────────────────────────────────
        table: {
            /**
             * Standart UBYS DataTable başlatır.
             * @param {string} selector  - CSS selector veya jQuery obj
             * @param {object} options   - DataTables options override
             */
            init(selector, options = {}) {
                if (typeof $.fn.DataTable === "undefined") return null;

                const defaults = {
                    language: {
                        url: "//cdn.datatables.net/plug-ins/1.13.8/i18n/tr.json",
                        emptyTable:  "Kayıt bulunamadı",
                        zeroRecords: "Eşleşen kayıt bulunamadı",
                    },
                    pageLength:    _cfg.tablePageLen,
                    lengthMenu:    [[10, 20, 50, 100], [10, 20, 50, 100]],
                    responsive:    true,
                    autoWidth:     false,
                    dom: "<'table-dt-top d-flex items-center justify-between mb-2'lf>" +
                         "<'table-responsive't>" +
                         "<'table-dt-bottom d-flex items-center justify-between mt-2'ip>",
                    drawCallback() {
                        // Row animasyonunu sıfırla
                        $(this.api().table().body()).find("tr").each((i, el) => {
                            el.style.animation = "none";
                            el.offsetHeight; // reflow
                            el.style.animation = "";
                        });
                    },
                };

                return $(selector).DataTable({ ...defaults, ...options });
            },

            /**
             * Mevcut DataTable'ı yeniden çizer.
             */
            reload(selector) {
                if (typeof $.fn.DataTable === "undefined") return;
                const dt = $(selector).DataTable();
                if (dt) dt.ajax.reload(null, false);
            },

            /**
             * Basit client-side tablo (ajax olmayan) için kısayol.
             */
            simple(selector, options = {}) {
                return this.init(selector, {
                    paging: true,
                    searching: true,
                    ordering: true,
                    ...options,
                });
            },
        },

        // ── Form Helpers ─────────────────────────────────────────────────
        form: {
            /**
             * Form alanlarını devre dışı bırakır ve loading gösterir.
             */
            setLoading(formEl, loading = true) {
                const btn = formEl.querySelector("[type='submit']");
                if (!btn) return;

                if (loading) {
                    btn.disabled = true;
                    btn._originalText = btn.innerHTML;
                    btn.innerHTML = `<i class="fas fa-spinner fa-spin me-2"></i>İşleniyor...`;
                } else {
                    btn.disabled = false;
                    if (btn._originalText) btn.innerHTML = btn._originalText;
                }
            },

            /**
             * AJAX ile form gönderir.
             */
            async submit(url, data, method = "POST") {
                const token = document.querySelector("input[name='__RequestVerificationToken']")?.value;
                const headers = { "Content-Type": "application/json" };
                if (token) headers["RequestVerificationToken"] = token;

                const response = await fetch(url, {
                    method,
                    headers,
                    body: JSON.stringify(data),
                });

                return response.json();
            },

            /**
             * Validation hatalarını forma uygular.
             */
            showErrors(errors) {
                // Önce temizle
                document.querySelectorAll(".form-control").forEach(el => {
                    el.classList.remove("is-invalid");
                });
                document.querySelectorAll(".invalid-feedback").forEach(el => {
                    el.textContent = "";
                });

                for (const [field, messages] of Object.entries(errors)) {
                    const input = document.querySelector(`[name='${field}']`);
                    if (!input) continue;
                    input.classList.add("is-invalid");
                    const feedback = input.nextElementSibling;
                    if (feedback?.classList.contains("invalid-feedback")) {
                        feedback.textContent = Array.isArray(messages) ? messages[0] : messages;
                    }
                }
            },
        },

        // ── Modal ────────────────────────────────────────────────────────
        modal: {
            show(id) {
                const overlay = document.getElementById(id);
                if (overlay) {
                    overlay.classList.add("show");
                    document.body.style.overflow = "hidden";
                }
            },
            hide(id) {
                const overlay = document.getElementById(id);
                if (overlay) {
                    overlay.classList.remove("show");
                    document.body.style.overflow = "";
                }
            },
            init() {
                // Overlay tıkla → kapat
                document.querySelectorAll(".modal-overlay").forEach(overlay => {
                    overlay.addEventListener("click", e => {
                        if (e.target === overlay) SMS.modal.hide(overlay.id);
                    });
                });
                // Close buton
                document.querySelectorAll(".modal-close").forEach(btn => {
                    btn.addEventListener("click", () => {
                        const overlay = btn.closest(".modal-overlay");
                        if (overlay) SMS.modal.hide(overlay.id);
                    });
                });
                // ESC tuşu
                document.addEventListener("keydown", e => {
                    if (e.key === "Escape") {
                        document.querySelectorAll(".modal-overlay.show").forEach(m => {
                            SMS.modal.hide(m.id);
                        });
                    }
                });
            },
        },

        // ── Yardımcı Fonksiyonlar ────────────────────────────────────────
        utils: {
            /**
             * Tarihi TR formatında döndürür: 12.06.2024
             */
            formatDate(dateStr) {
                if (!dateStr) return "-";
                try {
                    return new Date(dateStr).toLocaleDateString("tr-TR", {
                        day: "2-digit", month: "2-digit", year: "numeric",
                    });
                } catch (_) { return dateStr; }
            },

            /**
             * Sayıyı virgüllü yazar: 1234 → 1.234
             */
            formatNumber(n) {
                return Number(n).toLocaleString("tr-TR");
            },

            /**
             * GANO'ya göre CSS sınıfı döndürür.
             */
            ganoClass(gano) {
                if (gano >= 3.0) return "excellent";
                if (gano >= 2.0) return "good";
                if (gano >= 1.0) return "average";
                return "poor";
            },

            /**
             * Harf notuna göre badge sınıfı döndürür.
             */
            harfNotuBadge(harf) {
                return `badge badge-${harf}`;
            },

            /**
             * Clipboard'a kopyalar ve toast gösterir.
             */
            async copyToClipboard(text) {
                try {
                    await navigator.clipboard.writeText(text);
                    SMS.toast("Kopyalandı!", "success");
                } catch (_) {
                    SMS.toast("Kopyalama başarısız.", "error");
                }
            },

            /**
             * Doluluk oranına göre progress bar rengi.
             */
            kontenjanClass(oran) {
                if (oran >= 90) return "danger";
                if (oran >= 60) return "warning";
                return "success";
            },
        },
    };
})();

// ─── DOM HAZIR ────────────────────────────────────────────────────────────────
document.addEventListener("DOMContentLoaded", () => {
    SMS.init();
    SMS.modal.init();
});
