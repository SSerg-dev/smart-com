Ext.define('App.view.core.common.BaseTreeGrid', {
    extend: 'Ext.tree.Panel',
    alias: 'widget.basetreegrid',

    useArrows: true,
    rootVisible: false,

    cls: 'directorygrid',
    bodyCls: 'border-body',

    editorModel: 'Core.form.EditorWindowModel',

    enableColumnHide: false,
    enableColumnMove: false,
    columnLines: true,
    rowLines: true,

    animCollapse: false,
    animate: false,

    scroll: true,

    dockedItems: [{
        xtype: 'gridinfotoolbar',
        dock: 'bottom',
        cls: 'grid-info-toolbar',
        height: 10
    }],

    viewConfig: {
        overflowX: 'scroll',
        overflowY: 'scroll',
        enableTextSelection: true,
        trackOver: false,
        animate: false,
        preserveScrollOnRefresh: true,
        stripeRows: true,
        loadMask: {
            maskOnDisable: false
        },
        listeners: {
            addhorizontalscroll: function (view) {
                // Изменение высоты футера из-за появления горизонтальной прокрутки
                view.panel.down('gridinfotoolbar').addCls('grid-info-toolbar-has-scroll');
            }
        }
    },

    constructor: function () {
        var cfg = {
            onBindStore: this.onBindStoreHandler
        };

        if (Ext.isDefined(this.viewConfig)) {
            Ext.apply(this.viewConfig, cfg);
        } else {
            this.viewConfig = cfg;
        }
        this.callParent(arguments);
        if (!Ext.isEmpty(this.editorModel) && Ext.isString(this.editorModel)) {
            this.editorModel = Ext.create(this.editorModel, {
                tree: this
            });
        }
    },

    initComponent: function () {
        var me = this;

        me.callParent(arguments);

        this.on('viewready', function (grid) {
            var columns = grid.headerCt.query('gridcolumn');
            //tooltip для заголовка колонки
            Ext.Array.each(columns, function (c) {
                var tooltipText = Ext.isDefined(c.tooltip) && c.tooltip != "" ? c.tooltip : c.text;
                if (tooltipText != "" && c.el != null) {
                    Ext.create('Ext.tip.ToolTip', {
                        target: c.el,
                        preventLoseFocus: true,
                        trackMouse: true,
                        html: tooltipText
                    })
                }
            });
        });

        //App.Util.callWhenRendered(me, function () {
        //    if (!me.getPlugin('clearfilterbutton')) {
        //        me.getEl().insertFirst({
        //            tag: 'div',
        //            cls: 'scroll-curtain'
        //        });
        //    }
        //});
    },

    onBindStoreHandler: function (store) {
        if (!this.rendered) {
            this.on({
                single: true,
                afterrender: bindStoreToPaging
            });
        } else {
            bindStoreToPaging(this);
        }

        function bindStoreToPaging(gridview) {
            var infotoolbar = gridview.up('basetreegrid').down('gridinfotoolbar');

            if (infotoolbar) {
                infotoolbar.bindStore(store);
                infotoolbar.onLoad();
            }
        }
    }
});
