Ext.define('App.util.TPM.customLockedGrid', {
    extend: 'Ext.grid.Panel',

    mixins: {
        configurable: 'App.util.core.ConfigurableGrid'
    },

    alias: 'widget.customlockedgrid',
    enableColumnHide: false,
    enableLocking: true,
    //enableColumnMove: false,
    columnLines: true,
    cls: 'directorygrid customlockedgrid',
    scroll: true,
    bodyCls: 'border-body',

    editorModel: 'Core.form.EditorFormModel',

    selModel: {
        selType: 'extcheckboxmodellocked',
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
        overflowY: 'scroll',
        overflowX: 'scroll',
        enableTextSelection: true,
        trackOver: false,
        loadMask: {
            maskOnDisable: false
        },
        listeners: {
            addhorizontalscroll: function (view) {
                // Изменение высоты футера из-за появления горизонтальной прокрутки
                // view.panel.up('clientkpidata').down('gridinfotoolbar').addCls('grid-info-toolbar-has-scroll');
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
        ptype: 'clearfilterbuttonlocked',
        tooltipText: l10n.ns('core').value('filterBarClearButtonText'),
        iconCls: 'mdi mdi-filter-remove',
        lockableScope: 'top'
    }, {
        // При использовании буферезированного стора происходит автоматическое 
        // добавление плагина bufferedrenderer, что приводит к повторному вызову 
        // метода Init в этом плагине и неправильной работе прокрутки. Для устранения 
        // этой проблемы необходимо явно определить плагин bufferedrenderer.
        ptype: 'bufferedrenderer',
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
                var buttonEl = column.el.first().first();
                Ext.core.DomHelper.append(buttonEl, {
                    tag: 'div',
                    style: 'position: absolute; width: 16px; height: 16px; top: 30px; left: 3px; font-size: medium; color: #8a98a7;',
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
            var infotoolbar = gridview.up('customlockedgrid').down('gridinfotoolbar');

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

//Переопределение плагина
Ext.define('App.util.TPM.ClearFilterButtonLocked', {
    extend: 'Ext.ux.grid.plugin.ClearFilterButton',
    alias: 'plugin.clearfilterbuttonlocked',
    pluginId: 'clearfilterbuttonlocked',

    iconCls: '',
    baseIconCls: Ext.baseCSSPrefix + 'clear-filter-icon',
    buttonCls: Ext.baseCSSPrefix + 'clear-filter-button',
    disabledCls: Ext.baseCSSPrefix + 'clear-filter-button-disabled',

    onAfterRender: function () {
        if (!this.clearButtonEl) {
            var cls;
            if (Ext.isEmpty(this.getFilterBar().filterArray)) {
                cls = this.buttonCls + ' ' + this.disabledCls;
            }

            this.clearButtonEl = this.grid.getEl().insertFirst({
                tag: 'div',
                cls: cls || this.buttonCls,
                style: {
                    width: Ext.getScrollbarSize().width + 'px',
                    height: this.grid.normalGrid.headerCt.getHeight() + 'px'
                },
                cn: {
                    tag: 'div',
                    cls: this.baseIconCls + ' ' + this.iconCls
                }
            });

            this.tooltip = Ext.create('Ext.tip.ToolTip', {
                target: this.clearButtonEl,
                trackMouse: false,
                html: this.tooltipText
            });

            this.clearButtonEl.on('click', this.onMouseClickOnClearButton, this);
        }
    },

    onAfterLayout: function () {
        this.clearButtonEl.setHeight(this.grid.normalGrid.headerCt.getHeight());
    },
});

Ext.define('App.util.core.ExtendedCheckboxModelLocked', {
    extend: 'App.util.core.ExtendedCheckboxModel',
    alias: 'selection.extcheckboxmodellocked',

    onHeaderClick: function (headerCt, header, e) {
        var grid = header.up('customlockedgrid'),
            store = grid.getStore();

        if (store.getTotalCount() !== 0) {
            if (header.isCheckerHd) {
                e.stopEvent();
                var me = this;
                if (me.allSelected) {
                    me.deselectAllRecords(header);
                    me.allSelected = false
                } else {
                    me.selectAllRecords(header);
                    me.allSelected = true
                }
                delete me.preventFocus;
            }
        }
    },
});
