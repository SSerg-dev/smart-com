Ext.override(Ext.panel.Table, {
    //syncHorizontalScroll: function (left, setBody) {
    //    var me = this;

    //    setBody = setBody === true;
    //    // Only set the horizontal scroll if we've changed position,
    //    // so that we don't set this on vertical scrolls
    //    if (me.rendered && (setBody || left !== me.scrollLeftPos)) {
    //        me.headerCt.el.dom.scrollLeft = left;
    //        me.scrollLeftPos = left;

    //        //promo grid
    //        var widget = this.up('#promoGrid');
    //        if (widget) {
    //            var view = this.getView(),
    //                viewEl = $(view.getEl().dom),
    //                jspHorizontalBar = viewEl.find('.jspHorizontalBar'),
    //                jspTrack = jspHorizontalBar.find('.jspTrack'),
    //                jspDrag = jspHorizontalBar.find('.jspDrag'),
    //                gridTable = viewEl.find('.x-grid-table')[0],
    //                gridTableWidth = gridTable.clientWidth,
    //                wrapWidth = 1187,
    //                leftPos = jspDrag[0].style.left,
    //                numLeftPos = Number(leftPos.substring(0, leftPos.lastIndexOf("p"))),
    //                wrap = Ext.query('div#promo-rowexpander-wrap'),
    //                maxWrapPaddingLeft = gridTableWidth - wrapWidth - 22,
    //                k = (jspTrack[0].clientWidth - jspDrag[0].clientWidth) / (maxWrapPaddingLeft - 28);
    //            // k - коэффициент для пересчета сдвига панелей с данными промо в зависимости от размера горизонтального скролла
    //            k = k < 1 ? 1 / k : k;

    //            if ((k * numLeftPos) <= maxWrapPaddingLeft) {
    //                wrap.forEach(function (rec, i) {
    //                    rec.style.paddingLeft = (k * numLeftPos) + "px";
    //                });
    //            }
    //        }
    //    }
    //}
});