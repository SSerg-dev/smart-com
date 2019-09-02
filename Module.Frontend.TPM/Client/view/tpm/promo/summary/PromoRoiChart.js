Ext.define('App.view.tpm.promo.PromoRoiChart', {
    extend: 'Ext.chart.Chart',
    alias: 'widget.promoroichart',
    animate: false,
    insetPadding: 5,
    shadow: false,
    sprites: [],

    legend: {
        position: 'bottom',
        boxStroke: 'none',
        customLegend: true,
        itemHSpacing: 5,
        itemWSpacing: 10,
        gaugeMouse: true,
        labelColor: '#0000A0'
    },
    series: [{
        type: 'gauge',
        showInLegend: true,
        field: 'value',
        custom: true,
        strokeColor: '#FFE000',
        minimum: 0,
        maximum: 100,
        donut: 60,
        colorSet: ['#0000A0', '#00D7B8', '#BBBBBB']
    }],
    listeners: {
        resize: function (me) {
            var text = me.getStore().getAt(1).data.value,
                y = me.series.items[0].centerY + 5;
            y = this.drawCustomSprite(me, 1, text, y, 'bigSummaryText roispriteText');

            text = me.getStore().getAt(0).data.value;
            this.drawCustomSprite(me, 0, text, y, 'mediumSummaryText spriteText');
        },
        beforerender: function (me) {
            me.series.items[0].maximum = me.maximum;
        }
    },

    drawCustomSprite: function (me, i, text, y, cls) {
        var surface = me.surface;
        if (me.sprites[i]) {
            me.sprites[i].remove();
        }
        me.sprites[i] = surface.add({
            type: 'text',
            text: text + '%',
            x: 0,
            y: 0
        });
        me.sprites[i].show(true);
        me.sprites[i].addCls(cls);

        var bbox = me.sprites[i].getBBox();
        me.sprites[i].setAttributes({
            x: me.series.items[0].centerX - bbox.width / 2,
            y: y - bbox.height / 2,
        }, true);
        return (y - bbox.height);
    }
});
