Ext.define('App.view.core.common.DirectoryGrid', {
    extend: 'Ext.grid.Panel',

    mixins: {
        configurable: 'App.util.core.ConfigurableGrid'
    },

    alias: 'widget.directorygrid',
    enableColumnHide: false,
    //enableColumnMove: false,
    columnLines: true,
    cls: 'directorygrid',
    scroll: true,
    bodyCls: 'border-body',

    editorModel: 'Core.form.EditorFormModel',

    selModel: {
        selType: 'extcheckboxmodel',
        pruneRemoved: false,
        allowDeselect: false
    },

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

    plugins: [{
        ptype: 'filterbar',
        renderHidden: false,
        showShowHideButton: false,
        showClearAllButton: false,
        enableOperators: false
    }, {
        ptype: 'clearfilterbutton',
        tooltipText: l10n.ns('core').value('filterBarClearButtonText'),
        iconCls: 'mdi mdi-filter-remove'
    }, {
        // При использовании буферезированного стора происходит автоматическое 
        // добавление плагина bufferedrenderer, что приводит к повторному вызову 
        // метода Init в этом плагине и неправильной работе прокрутки. Для устранения 
        // этой проблемы необходимо явно определить плагин bufferedrenderer.
        ptype: 'bufferedrenderer'
    }, {
        ptype: 'maskbinder',
        msg: l10n.ns('core').value('loadingText')
    }],

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
        this.mixins.configurable.constructor.call(this, arguments);

        if (!Ext.isEmpty(this.editorModel) && Ext.isString(this.editorModel)) {
            this.editorModel = Ext.create(this.editorModel, {
                grid: this
            });
        }
    },

    initComponent: function () {
        this.addEvents('afterbindstore');
        this.callParent(arguments);
        this.on('viewready', function (grid) {

            var columns = grid.headerCt.query('gridcolumn');
            if (grid.multiSelect) {
                if (columns.length > 0 && columns[0].hasOwnProperty('isCheckerHd')) {
                    columns[0].show();
                }
            }
            //tooltip для заголовка колонки
            Ext.Array.each(columns, function (c) {
                var tooltipText = Ext.isDefined(c.tooltip) && c.tooltip != "" ? c.tooltip : c.text;
                if (tooltipText != "" && c.el != null && !c.isCheckerHd) {
                    Ext.create('Ext.tip.ToolTip', {
                        target: c.el,
                        preventLoseFocus: true,
                        trackMouse: true,
                        html: tooltipText
                    })
                }
            });

            var column = columns[0];
            if (column.isCheckerHd && column.el != null) {
                Ext.core.DomHelper.append(column.el, {
                    tag: 'div',
                    style: 'position: absolute; width: 16px; height: 16px; top: 30px; left: 6px; font-size: medium; color: #8a98a7;',
                    cls: 'mdi mdi-check-all',
                    'data-qtip': l10n.ns('core', 'DirectoryGrid').value('SelectAllToolTip'),
                });
            }
        });
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
            var infotoolbar = gridview.up('grid').down('gridinfotoolbar');

            if (infotoolbar) {
                infotoolbar.bindStore(store);
                infotoolbar.onLoad();
            }
        }
    },

    bindStore: function (store) {
        if (store.isExtendedStore) {
            Ext.destroy(this.extFilterRelayedEvents);
            this.extFilterRelayedEvents = this.relayEvents(store, ['extfilterchange']);
        }

        Ext.destroy(this.relayedEvents);
        this.relayedEvents = this.relayEvents(store, ['load']);

        this.callParent(arguments);
        this.fireEvent('afterbindstore', this, store);
    },

    setLoadMaskDisabled: function (disabled) {
        var funcName = disabled ? 'disable' : 'enable';
        var loadMask = this.getView().loadMask;
        if (loadMask[funcName]) {
            loadMask[funcName]();
        }
        if (this.maskBinder) {
            this.maskBinder.disableShowMask(disabled);

        }
    }

});
