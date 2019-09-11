Ext.define('Ext.ux.grid.plugin.ClearFilterButton', {
    extend: 'Ext.AbstractPlugin',
    alias: 'plugin.clearfilterbutton',
    pluginId: 'clearfilterbutton',

    iconCls: '',
    baseIconCls: Ext.baseCSSPrefix + 'clear-filter-icon',
    buttonCls: Ext.baseCSSPrefix + 'clear-filter-button',
    disabledCls: Ext.baseCSSPrefix + 'clear-filter-button-disabled',

    tooltipText: 'Clear',

    grid: undefined,

    constructor: function (cfg) {
        Ext.apply(this, cfg);
        this.callParent(arguments);

    },

    init: function (grid) {
        this.grid = grid;

        if (!grid.rendered) {
            grid.on('afterrender', this.onAfterRender, this);
        }
        else {
            this.onAfterRender();
        }

        grid.on('afterlayout', this.onAfterLayout, this);
        grid.on('beforefilterupdate', this.updateClearButtonsVisibility, this);
        grid.on('filterupdated', this.updateClearButtonsVisibility, this);
    },

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
                    height: this.grid.headerCt.getHeight() + 'px'
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
        this.clearButtonEl.setHeight(this.grid.headerCt.getHeight());
    },

    onMouseClickOnClearButton: function (event, htmlElement, object) {
        var filterbar = this.getFilterBar();

        if (filterbar) {
            filterbar.clearFilters();
        }
    },

    updateClearButtonsVisibility: function () {
        var filterbar = this.getFilterBar(),
            fields = filterbar.fields,
            values = Ext.Array.clean(
                fields.getRange().map(function (item) {
                    return filterbar.grid.store.remoteFilter ? item.getSubmitValue() : item.getValue();
                })
            );

        if (Ext.isEmpty(values)) {
            this.clearButtonEl.addCls(this.disabledCls);

            fields.each(function (field) {
                var clearBtn = field.getPlugin('fieldclearbutton');
                if (clearBtn) {
                    clearBtn.updateClearButtonVisibility();
                }
            });
        } else {
            this.clearButtonEl.removeCls(this.disabledCls);
        }
    },

    getFilterBar: function () {
        return this.grid.getPlugin('filterbar');
    }
});
