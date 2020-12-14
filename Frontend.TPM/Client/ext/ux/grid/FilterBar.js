/**
 * Plugin that enable filters on the grid headers.<br>
 * The header filters are integrated with new Ext4 <code>Ext.data.Store</code> filters.<br>
 *
 * @author Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * @version 1.1 (supports 4.1.1)
 * @updated 2011-10-18 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Support renderHidden config option, isVisible(), and setVisible() methods (added getFilterBar() method to the grid)
 * Fix filter bug that append filters to Store filters MixedCollection
 * Fix layout broken on initial render when columns have width property
 * @updated 2011-10-24 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Rendering code rewrited, filters are now rendered inside de column headers, this solves scrollable grids issues, now scroll, columnMove, and columnHide/Show is handled by the headerCt
 * Support showClearButton config option, render a clear Button for each filter to clear the applied filter (uses Ext.ux.form.field.ClearButton plugin)
 * Added clearFilters() method.
 * @updated 2011-10-25 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Allow preconfigured filter's types and auto based on store field data types
 * Auto generated stores for combo and list filters (local collect or server in autoStoresRemoteProperty response property)
 * @updated 2011-10-26 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Completelly rewriten to support reconfigure filters on grid's reconfigure
 * Supports clearAll and showHide buttons rendered in an actioncolumn or in new generetad small column
 * @updated 2011-10-27 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Added support to 4.0.7 (columnresize not fired correctly on this build)
 * @updated 2011-11-02 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Filter on ENTER
 * Defaults submitFormat on date filter to 'Y-m-d' and use that in applyFilters for local filtering
 * Added null value support on combo and list filters (autoStoresNullValue and autoStoresNullText)
 * Fixed some combo styles
 * @updated 2011-11-10 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Parse and show initial filters applied to the store (only property -> value filters, filterFn is unsuported)
 * @updated 2011-12-12 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Extends AbstractPlugin and use Observable as a Mixin
 * Yes/No localization on constructor
 * @updated 2012-01-03 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Added some support for 4.1 beta
 * @updated 2012-01-05 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * 99% support for 4.1 beta. Seems to be working
 * @updated 2012-03-22 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Fix focusFirstField method
 * Allow to specify listConfig in combo filter
 * Intercept column's setPadding for all columns except actionColumn or extraColumn (fix checkBoxSelectionModel header)
 * @updated 2012-05-07 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Fully tested on 4.1 final
 * @updated 2012-05-31 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Fix padding issue on checkbox column
 * @updated 2012-07-10 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Add msgTarget: none to field to fix overridding msgTarget to side in fields in 4.1.1
 * @updated 2012-07-26 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Fixed sort on enter bug regression
 * add checkChangeBuffer: 50 to field, this way works as expected if this config is globally overridden
 * private method applyFilters refactored to support delayed (key events) and instant filters (enter key and combo/picker select event)
 * @updated 2012-07-31 by Ing. Leonardo D'Onofrio (leonardo_donofrio at hotmail.com)
 * Added operator selection in number and date filters
*/

Ext.define('Ext.ux.grid.FilterBar', {
    extend: 'Ext.AbstractPlugin',
    alias: 'plugin.filterbar',
    uses: [
        'Ext.window.MessageBox',
        'Ext.ux.form.field.ClearButton',
        'Ext.ux.form.field.OperatorButton',
        'Ext.container.Container',
        'Ext.util.DelayedTask',
        'Ext.layout.container.HBox',
        'Ext.data.ArrayStore',
        'Ext.button.Button',
        'Ext.form.field.Text',
        'Ext.form.field.Number',
        'Ext.form.field.Date',
        'Ext.form.field.ComboBox'
    ],
    mixins: {
        observable: 'Ext.util.Observable'
    },

    pluginId: 'filterbar',
    updateBuffer: 800,					// buffer time to apply filtering when typing/selecting

    columnFilteredCls: Ext.baseCSSPrefix + 'column-filtered', // CSS class to apply to the filtered column header

    needFocus: true, //need focus first field 
    renderHidden: true,					// renders the filters hidden by default, use in combination with showShowHideButton
    showShowHideButton: true,					// add show/hide button in actioncolumn header if found, if not a new small column is created
    showHideButtonTooltipDo: 'Показать фильры',	// button tooltip show
    showHideButtonTooltipUndo: 'Скрыть фильтры',	// button tooltip hide
    showHideButtonIconCls: 'filter',				// button iconCls

    showClearButton: true,					// use Ext.ux.form.field.ClearButton to allow user to clear each filter, the same as showShowHideButton
    showClearAllButton: true,					// add clearAll button in actioncolumn header if found, if not a new small column is created
    clearAllButtonIconCls: 'clear-filters', 		// css class with the icon of the clear all button
    clearAllButtonTooltip: 'Очистить все',	// button tooltip

    autoStoresRemoteProperty: 'autoStores',			// if no store is configured for a combo filter then stores are created automatically, if remoteFilter is true then use this property to return arrayStores from the server
    autoStoresNullValue: '###NULL###',			// value send to the server to expecify null filter
    autoStoresNullText: '[пусто]',			// NULL Display Text
    autoUpdateAutoStores: false,				// if set to true combo autoStores are updated each time that a filter is applied

    enableOperators: true,					// enable operator selection for number and date filters

    boolTpl: {
        xtype: 'combo',
        queryMode: 'local',
        forceSelection: true,
        triggerAction: 'all',
        editable: false,
        store: [
            [1, l10n.ns('core', 'booleanValues').value('true'),],
            [0, l10n.ns('core', 'booleanValues').value('false'),]
        ],
        operator: 'eq'
    },
    dateTpl: {
        xtype: 'datefield',
        editable: false,
        submitFormat: 'Y-m-d',
        operator: 'eq'
    },
    datetimeTpl: {
        xtype: 'datetimefield',
        showClearBtn: false,
        editable: false
    },
    floatTpl: {
        xtype: 'numberfield',
        submitLocaleSeparator: false,
        decimalPrecision: 2,
        allowDecimals: true,
        decimalSeparator: ',',
        hideTrigger: true,
        keyNavEnabled: false,
        mouseWheelEnabled: false,
        operator: 'eq'
    },
    intTpl: {
        xtype: 'numberfield',
        allowDecimals: false,
        minValue: 0,
        operator: 'eq'
    },
    stringTpl: {
        xtype: 'textfield',
        operator: 'like'
    },
    searchTpl: {
        xtype: 'fsearchfield',
        trigger2Cls: '',
        multiSelect: true,
        operator: 'like'
    },
    combosearchTpl: {
        xtype: 'fsearchcombobox',
        trigger3Cls: '',
        operator: 'eq'
    },
    comboTpl: {
        xtype: 'combo',
        queryMode: 'local',
        forceSelection: true,
        editable: false,
        triggerAction: 'all',
        operator: 'eq'
    },
    listTpl: {
        xtype: 'combo',
        queryMode: 'local',
        forceSelection: true,
        editable: false,
        triggerAction: 'all',
        multiSelect: true,
        operator: 'in'
    },

    constructor: function () {
        var me = this;

        me.mixins.observable.constructor.call(me);
        me.callParent(arguments);
    },

    // private
    init: function (grid) {
        var me = this;

        grid.on({
            columnresize: me.resizeContainer,
            columnhide: me.resizeContainer,
            columnshow: me.resizeContainer,
            beforedestroy: me.unsetup,
            reconfigure: me.resetup,
            scope: me
        });

        grid.addEvents('beforefilterupdate', 'filterupdated');

        Ext.apply(grid, {
            filterBar: me,
            getFilterBar: function () {
                return this.filterBar;
            }
        });

        me.setup(grid);
    },

    // private
    setup: function (grid) {
        var me = this;

        me.grid = grid;
        me.visible = !me.renderHidden;
        me.autoStores = Ext.create('Ext.util.MixedCollection');
        me.autoStoresLoaded = false;
        me.columns = Ext.create('Ext.util.MixedCollection');
        me.containers = Ext.create('Ext.util.MixedCollection');
        me.fields = Ext.create('Ext.util.MixedCollection');
        me.actionColumn = me.grid.down('actioncolumn') || me.grid.down('actioncolumnpro');
        me.extraColumn = null;
        me.clearAllEl = null;
        me.showHideEl = null;
        me.task = Ext.create('Ext.util.DelayedTask');
        me.filterArray = [];

        me.overrideProxy();
        me.parseFiltersConfig(); 	// sets me.columns and me.autoStores
        me.parseInitialFilters();   // sets me.filterArray with the store previous filters if any (adds operator and type if missing)
        me.renderExtraColumn(); 	// sets me.extraColumn if applicable

        // renders the filter's bar
        if (grid.rendered) {
            me.renderFilterBar(grid);
        } else {
            grid.on('afterrender', me.renderFilterBar, me, { single: true });
        }
    },

    // private
    unsetup: function (grid) {
        var me = this;

        if (me.autoStores.getCount()) {
            me.grid.store.un('load', me.fillAutoStores, me);
        }

        me.autoStores.each(function (item) {
            Ext.destroy(item);
        });
        me.autoStores.clear();
        me.autoStores = null;
        me.columns.each(function (column) {
            if (column.rendered) {
                if (column.getEl().hasCls(me.columnFilteredCls)) {
                    column.getEl().removeCls(me.columnFilteredCls);
                }
            }
        }, me);
        me.columns.clear();
        me.columns = null;
        me.fields.each(function (item) {
            Ext.destroy(item);
        });
        me.fields.clear();
        me.fields = null;
        me.containers.each(function (item) {
            Ext.destroy(item);
        });
        me.containers.clear();
        me.containers = null;
        if (me.clearAllEl) {
            Ext.destroy(me.clearAllEl);
            me.clearAllEl = null;
        }
        if (me.showHideEl) {
            Ext.destroy(me.showHideEl);
            me.showHideEl = null;
        }
        if (me.extraColumn) {
            me.grid.headerCt.items.remove(me.extraColumn);
            Ext.destroy(me.extraColumn);
            me.extraColumn = null;
        }
        me.task = null;
        me.filterArray = null;
    },

    // private
    resetup: function (grid) {
        var me = this;

        me.unsetup(grid);
        me.setup(grid);
    },

    // private
    overrideProxy: function () {
        var me = this;

        //override encodeFilters to append type and operator in remote filtering
        Ext.apply(me.grid.store.proxy, {
            encodeFilters: function (filters) {
                var min = [],
                    length = filters.length,
                    i = 0;

                for (; i < length; i++) {
                    min[i] = {
                        property: filters[i].property,
                        value: filters[i].value
                    };
                    if (filters[i].type) {
                        min[i].type = filters[i].type;
                    }
                    if (filters[i].operator) {
                        min[i].operator = filters[i].operator;
                    }
                }
                return this.applyEncoding(min);
            }
        });
    },

    // private
    parseFiltersConfig: function () {
        var me = this;
        var columns = this.grid.query('gridcolumn');
        me.columns.clear();
        me.autoStores.clear();
        Ext.each(columns, function (column) {
            if (column.filter) {
                if (column.filter === true || column.filter === 'auto') { // automatic types configuration (store based)
                    var type = me.grid.store.model.prototype.fields.get(column.dataIndex).type.type;
                    if (type == 'auto') type = 'string';
                    column.filter = type;
                }
                if (Ext.isString(column.filter)) {
                    column.filter = {
                        type: column.filter // only set type to then use templates
                    };
                }
                if (column.filter.type) {
                    column.filter = Ext.applyIf(column.filter, me[column.filter.type + 'Tpl']); // also use templates but with user configuration
                }

                if (column.filter.xtype == 'combo' && !column.filter.store) {
                    column.autoStore = true;
                    column.filter.store = Ext.create('Ext.data.ArrayStore', {
                        fields: [{
                            name: 'text'
                        }, {
                            name: 'id'
                        }]
                    });
                    me.autoStores.add(column.dataIndex, column.filter.store);
                    column.filter = Ext.apply(column.filter, {
                        displayField: 'text',
                        valueField: 'id'
                    });
                }

                if (!column.filter.type) {
                    switch (column.filter.xtype) {
                        case 'combo':
                            column.filter.type = (column.filter.multiSelect ? 'list' : 'combo');
                            break;
                        case 'fsearchfield':
                            column.filter.type = 'search';
                            break;
                        case 'datefield':
                            column.filter.type = 'date';
                            break;
                        case 'datetimefield':
                            column.filter.type = 'datetime';
                            break;
                        case 'numberfield':
                            column.filter.type = (column.filter.allowDecimals ? 'float' : 'int');
                            break;
                        default:
                            column.filter.type = 'string';
                    }
                }

                if (!column.filter.operator) {
                    column.filter.operator = me[column.filter.type + 'Tpl'].operator;
                }

                if (column.filter.type == 'search' && column.filter.store) {
                    column.filter.store.autoLoad = false;

                    var fields = Ext.ModelManager.getModel(column.filter.store.model).getFields(),
                        field = Ext.Array.findBy(fields, function (item) {
                            return item.name === column.filter.valueField;
                        });

                    column.filter.valueType = field.type.type;
                    if (field && column.filter.valueType !== 'string') {
                        column.filter.operator = 'eq';
                    }
                }

                me.columns.add(column.dataIndex, column);
            }
        }, me);
        if (me.autoStores.getCount()) {
            if (me.grid.store.getCount() > 0) {
                me.fillAutoStores(me.grid.store);
            }
            if (me.grid.store.remoteFilter) {
                var autoStores = [];
                me.autoStores.eachKey(function (key, item) {
                    autoStores.push(key);
                });
                me.grid.store.proxy.extraParams = me.grid.store.proxy.extraParams || {};
                me.grid.store.proxy.extraParams[me.autoStoresRemoteProperty] = autoStores;
            }
            me.grid.store.on('load', me.fillAutoStores, me);
        }
    },

    // private
    fillAutoStores: function (store) {
        var me = this;

        if (!me.autoUpdateAutoStores && me.autoStoresLoaded) return;

        me.autoStores.eachKey(function (key, item) {
            var field = me.fields.get(key);
            if (field) {
                field.suspendEvents();
                var fieldValue = field.getValue();
            }
            if (!store.remoteFilter) { // values from local store
                var data = store.collect(key, true, false).sort();
                var records = [];
                Ext.each(data, function (txt) {
                    if (Ext.isEmpty(txt)) {
                        Ext.Array.insert(records, 0, [{
                            text: me.autoStoresNullText,
                            id: me.autoStoresNullValue
                        }]);
                    } else {
                        records.push({
                            text: txt,
                            id: txt
                        });
                    }
                });
                item.loadData(records);
            } else { // values from server
                if (store.proxy.reader.rawData[me.autoStoresRemoteProperty]) {
                    var data = store.proxy.reader.rawData[me.autoStoresRemoteProperty];
                    if (data[key]) {
                        var records = [];
                        Ext.each(data[key].sort(), function (txt) {
                            if (Ext.isEmpty(txt)) {
                                Ext.Array.insert(records, 0, [{
                                    text: me.autoStoresNullText,
                                    id: me.autoStoresNullValue
                                }]);
                            } else {
                                records.push({
                                    text: txt,
                                    id: txt
                                });
                            }
                        });
                        item.loadData(records);
                    }
                }
            }
            if (field) {
                field.setValue(fieldValue);
                field.resumeEvents();
            }
        }, me);
        me.autoStoresLoaded = true;
        if (me.grid.store.remoteFilter && !me.autoUpdateAutoStores) {
            delete me.grid.store.proxy.extraParams[me.autoStoresRemoteProperty];
        }
    },

    // private
    parseInitialFilters: function () {
        var me = this;

        me.filterArray = [];
        me.grid.store.filters.each(function (filter) {
            // try to parse initial filters, for now filterFn is unsuported
            if (filter.property && !Ext.isEmpty(filter.value) && me.columns.get(filter.property)) {
                if (!filter.type) filter.type = me.columns.get(filter.property).filter.type;
                if (!filter.operator) filter.operator = me.columns.get(filter.property).filter.operator;
                me.filterArray.push(filter);
            }
        }, me);
    },

    // private
    renderExtraColumn: function () {
        var me = this;

        if (me.columns.getCount() && !me.actionColumn && (me.showClearAllButton || me.showShowHideButton)) {
            var extraColumnCssClass = Ext.baseCSSPrefix + 'filter-bar-extra-column-hack';
            if (!document.getElementById(extraColumnCssClass)) {
                var style = document.createElement('style');
                var css = 'tr.' + Ext.baseCSSPrefix + 'grid-row td.' + extraColumnCssClass + ' { background-color: #ffffff !important; border-color: #ffffff !important; }';
                style.setAttribute('type', 'text/css');
                style.setAttribute('id', extraColumnCssClass);
                document.body.appendChild(style);
                if (style.styleSheet) {   	// IE
                    style.styleSheet.cssText = css;
                } else {                	// others
                    var cssNode = document.createTextNode(css);
                    style.appendChild(cssNode);
                }
            }
            me.extraColumn = Ext.create('Ext.grid.column.Column', {
                draggable: false,
                hideable: false,
                menuDisabled: true,
                sortable: false,
                resizable: false,
                fixed: true,
                width: 28,
                minWidth: 28,
                maxWidth: 28,
                header: '&nbsp;'//,
                //tdCls: extraColumnCssClass
            });
            me.grid.headerCt.add(me.extraColumn);
        }
    },

    // private
    renderFilterBar: function (grid) {
        var me = this;

        me.containers.clear();
        me.fields.clear();
        me.columns.eachKey(function (key, column) {
            var listConfig = column.filter.listConfig || {};
            listConfig = Ext.apply(listConfig, {
                style: 'border-top-width: 1px'
            });
            var plugins = [];
            if (me.showClearButton) {
                plugins.push({
                    ptype: 'fieldclearbutton',
                    listeners: {
                        clear: me.applyInstantFilters,
                        scope: me
                    }
                });
            }
            if (me.enableOperators && (column.filter.type == 'date' || column.filter.type == 'int' || column.filter.type == 'float')) {
                plugins.push({
                    ptype: 'operatorbutton',
                    listeners: {
                        operatorchanged: function (txt) {
                            if (Ext.isEmpty(txt.getValue())) return;
                            me.applyInstantFilters(txt);
                        }
                    }
                });
            }
            var field = Ext.widget(column.filter.xtype, Ext.apply(column.filter, {
                dataIndex: key,
                flex: 1,
                margin: 0,
                listConfig: listConfig,
                preventMark: true,
                msgTarget: 'none',
                checkChangeBuffer: 50,
                enableKeyEvents: true,
                plugins: plugins,

                listeners: {
                    change: function (f, newVal, oldVal) {
                        field.setApplied(false);
                    },
                    blur: me.applyInstantFilters,
                    select: me.applyInstantFilters,
                    keypress: function (txt, e) {
                        if (e.getCharCode() == 13) {
                            e.stopEvent();
                            me.applyInstantFilters(txt);
                        }
                        return false;
                    },
                    scope: me
                },

                _isApplied: true,
                isApplied: function () {
                    return this._isApplied;
                },
                setApplied: function (val) {
                    this._isApplied = val;
                },

                clearFilter: function () {
                    field.reset();
                }
            }));

            me.fields.add(column.dataIndex, field);
            var container = Ext.create('Ext.container.Container', {
                dataIndex: key,
                layout: 'hbox',
                cls: 'fileterbar-field-container',
                bodyStyle: 'background-color: "transparent";',
                width: column.getWidth(),
                items: [field],
                padding: '0 4 3 4',
                listeners: {
                    scope: me,
                    element: 'el',
                    mousedown: function (e) { e.stopPropagation(); },
                    click: function (e) { e.stopPropagation(); },
                    dblclick: function (e) { e.stopPropagation(); },
                    keydown: function (e) { e.stopPropagation(); },
                    keypress: function (e) { e.stopPropagation(); },
                    keyup: function (e) { e.stopPropagation(); }
                }
            });
            me.containers.add(column.dataIndex, container);
            container.render(Ext.get(column.id));
        }, me);
        var excludedCols = [];
        if (me.actionColumn) excludedCols.push(me.actionColumn.id);
        if (me.extraColumn) excludedCols.push(me.extraColumn.id);
        Ext.each(me.grid.query('gridcolumn'), function (column) {
            if (!Ext.Array.contains(excludedCols, column.id)) {
                column.setPadding = Ext.Function.createInterceptor(column.setPadding, function (h) {
                    if (column.hasCls(Ext.baseCSSPrefix + 'column-header-checkbox')) { //checkbox column
                        this.titleEl.setStyle({
                            paddingTop: '4px'
                        });
                    }
                    return false;
                });
            }
        });


        me.setVisible(me.visible);

        me.renderButtons();

        me.showInitialFilters();
    },

    //private
    renderButtons: function () {
        var me = this;

        if (me.showShowHideButton && me.columns.getCount()) {
            var column = me.actionColumn || me.extraColumn;
            var buttonEl = column.el.first().first();
            me.showHideEl = Ext.get(Ext.core.DomHelper.append(buttonEl, {
                tag: 'div',
                style: 'position: absolute; width: 16px; height: 16px; top: 3px; cursor: pointer; left: ' + parseInt((column.el.getWidth() - 16) / 2) + 'px',
                cls: me.showHideButtonIconCls,
                'data-qtip': (me.renderHidden ? me.showHideButtonTooltipDo : me.showHideButtonTooltipUndo)
            }));
            me.showHideEl.on('click', function () {
                me.setVisible(!me.isVisible());
                me.showHideEl.set({
                    'data-qtip': (!me.isVisible() ? me.showHideButtonTooltipDo : me.showHideButtonTooltipUndo)
                });
            });
        }

        if (me.showClearAllButton && me.columns.getCount()) {
            var column = me.actionColumn || me.extraColumn;
            var buttonEl = column.el.first().first();
            me.clearAllEl = Ext.get(Ext.core.DomHelper.append(buttonEl, {
                tag: 'div',
                style: 'position: absolute; width: 24px; height: 24px; top: 40px; cursor: pointer; left: ' + parseInt((column.el.getWidth() - 28) / 2) + 'px',
                cls: me.clearAllButtonIconCls,
                'data-qtip': me.clearAllButtonTooltip
            }));

            me.clearAllEl.hide();
            me.clearAllEl.on('click', function () {
                me.clearFilters();
            });
        }
    },

    // private
    showInitialFilters: function () {
        var me = this;

        Ext.each(me.filterArray, function (filter) {
            var column = me.columns.get(filter.property);
            var field = me.fields.get(filter.property);
            if (!column.getEl().hasCls(me.columnFilteredCls)) {
                column.getEl().addCls(me.columnFilteredCls);
            }
            field.suspendEvents();
            field.setValue(filter.value);
            field.resumeEvents();
        });

        if (me.filterArray.length && me.showClearAllButton) {
            me.clearAllEl.show({ duration: 1000 });
        }
    },

    // private
    resizeContainer: function (headerCt, col) {
        var me = this;
        var dataIndex = col.dataIndex;

        if (!dataIndex) return;
        var item = me.containers.get(dataIndex);
        if (item && item.rendered) {
            var itemWidth = item.getWidth();
            var colWidth = me.columns.get(dataIndex).getWidth();
            if (itemWidth != colWidth) {
                item.setWidth(me.columns.get(dataIndex).getWidth());
                item.doLayout();
            }
        }
    },

    // private
    applyFilters: function (field) {
        if (!field.isValid()) return;
        var me = this,
            grid = me.grid,
            column = me.columns.get(field.dataIndex),
            newVal = (grid.store.remoteFilter ? field.getSubmitValue() : field.getValue());

        if (Ext.isArray(newVal) && newVal.length != 0) {
            if (newVal.indexOf('Interface') != -1) {
                var index = newVal.indexOf('Interface');
                newVal[index] = newVal[index].toLowerCase();
            }
        } else if (newVal === 'Interface') {
            newVal = newVal.toLowerCase();
        }

        if (Ext.isArray(newVal) && newVal.length == 0) {
            newVal = '';
        }
        /*var myIndex = -1;
        Ext.each(me.filterArray, function (item2, index, allItems) {
            if (item2.property === column.dataIndex) {
                myIndex = index;
            }
        });
        if (myIndex != -1) {
            me.filterArray.splice(myIndex, 1);
        }*/

        for (i = me.filterArray.length - 1; i >= 0; i -= 1) {
            if (me.filterArray[i].property === column.dataIndex) {
                me.filterArray.splice(i, 1);
            }
        }

        if (!Ext.isEmpty(newVal)) {
            if (!grid.store.remoteFilter) {
                var operator = field.operator || column.filter.operator,
                    filterFn;
                switch (operator) {
                    case 'eq':
                        filterFn = function (item) {
                            if (column.filter.type == 'date') {
                                return Ext.Date.clearTime(item.get(column.dataIndex), true).getTime() == Ext.Date.clearTime(newVal, true).getTime();
                            } else {
                                return (Ext.isEmpty(item.get(column.dataIndex)) ? me.autoStoresNullValue : item.get(column.dataIndex)) == (Ext.isEmpty(newVal) ? me.autoStoresNullValue : newVal);
                            }
                        };
                        break;
                    case 'gte':
                        filterFn = function (item) {
                            if (column.filter.type == 'date') {
                                return Ext.Date.clearTime(item.get(column.dataIndex), true).getTime() >= Ext.Date.clearTime(newVal, true).getTime();
                            } else {
                                return (Ext.isEmpty(item.get(column.dataIndex)) ? me.autoStoresNullValue : item.get(column.dataIndex)) >= (Ext.isEmpty(newVal) ? me.autoStoresNullValue : newVal);
                            }
                        };
                        break;
                    case 'lte':
                        filterFn = function (item) {
                            if (column.filter.type == 'date') {
                                return Ext.Date.clearTime(item.get(column.dataIndex), true).getTime() <= Ext.Date.clearTime(newVal, true).getTime();
                            } else {
                                return (Ext.isEmpty(item.get(column.dataIndex)) ? me.autoStoresNullValue : item.get(column.dataIndex)) <= (Ext.isEmpty(newVal) ? me.autoStoresNullValue : newVal);
                            }
                        };
                        break;
                    case 'ne':
                        filterFn = function (item) {
                            if (column.filter.type == 'date') {
                                return Ext.Date.clearTime(item.get(column.dataIndex), true).getTime() != Ext.Date.clearTime(newVal, true).getTime();
                            } else {
                                return (Ext.isEmpty(item.get(column.dataIndex)) ? me.autoStoresNullValue : item.get(column.dataIndex)) != (Ext.isEmpty(newVal) ? me.autoStoresNullValue : newVal);
                            }
                        };
                        break;
                    case 'like':
                        filterFn = function (item) {
                            var re = new RegExp(newVal, 'i');
                            return re.test(item.get(column.dataIndex));
                        };
                        break;
                    case 'in':
                        filterFn = function (item) {
                            var re = new RegExp('^' + newVal.join('|') + '$', 'i');
                            return re.test((Ext.isEmpty(item.get(column.dataIndex)) ? me.autoStoresNullValue : item.get(column.dataIndex)));
                        };
                        break;
                }
                me.filterArray.push(Ext.create('Ext.util.Filter', {
                    property: column.dataIndex,
                    filterFn: filterFn,
                    me: me
                }));
            } else {
                if (column.filter.type == 'date') {
                    var from = Ext.Date.clearTime(new Date(newVal), true);
                    var store = grid.getStore();
                    var timeZone = null;

                    if (store.model && store.model.getFields) {
                        store.model.getFields().forEach(function (fieldModel) {
                            if (fieldModel.name == column.dataIndex && fieldModel.timeZone)
                                timeZone = fieldModel.timeZone;
                        });
                    }

                    if (timeZone !== null) {
                        var fromClear = Ext.Date.parse(newVal, "Y-m-d");
                        var currentTimeZone = from.getTimezoneOffset() / -60;
                        var hour = timeZone - currentTimeZone;
                        var date = new Date(fromClear.getFullYear(), fromClear.getMonth(), fromClear.getDate(), 0, 0, 0);

                        date.setHours(date.getHours() - hour);
                        from = date;
                    }

                    me.filterArray.push(Ext.create('Ext.util.Filter', {
                        property: column.dataIndex,
                        value: from.toUTCString(),
                        type: column.filter.type,
                        operator: 'gte'
                    }));

                    me.filterArray.push(Ext.create('Ext.util.Filter', {
                        property: column.dataIndex,
                        value: Ext.Date.add(Ext.Date.add(from, Ext.Date.DAY, 1), Ext.Date.MILLI, -1).toUTCString(),
                        type: column.filter.type,
                        operator: 'lte'
                    }));
                } else if (column.filter.type == 'datetime') {
                    var from = new Date(newVal);
                    from.setSeconds(0);
                    from.setMilliseconds(0);

                    me.filterArray.push(Ext.create('Ext.util.Filter', {
                        property: column.dataIndex,
                        value: from.toUTCString(),
                        type: column.filter.type,
                        operator: 'gte'
                    }));

                    me.filterArray.push(Ext.create('Ext.util.Filter', {
                        property: column.dataIndex,
                        value: Ext.Date.add(Ext.Date.add(from, Ext.Date.MINUTE, 1), Ext.Date.MILLI, -1).toUTCString(),
                        type: column.filter.type,
                        operator: 'lte'
                    }));
                } else if (column.filter.type == 'float') {
                    //пустое значине с n колиеством знаков после запятой (0,000)
                    var emptyVal = newVal.includes('.') ? '0.' + newVal.split('.')[1].replace(/[0-9]/g, '0') : 0;
                    //минимальное значение входной строки (0,001)
                    var minVal = emptyVal.toString().substring(0, emptyVal.length - 1) + '1';
                    //так как format округляет  числа до большего преобразуем минимальный порог(0.15 в 0.145) 
                    var roundingValue = minVal.length > 3 ? emptyVal.toString() + '5' : '0';
                    var updateVal = parseFloat(newVal) + parseFloat(minVal) - parseFloat(roundingValue);
                    var gteValue = newVal - parseFloat(roundingValue);

                    gteValue = gteValue.toFixed(minVal.length - 1);
                    updateVal = updateVal.toFixed(minVal.length - 1);

                    me.filterArray.push(Ext.create('Ext.util.Filter', {
                        property: column.dataIndex,
                        value: gteValue,
                        type: column.filter.type,
                        operator: 'gte'
                    }));

                    me.filterArray.push(Ext.create('Ext.util.Filter', {
                        property: column.dataIndex,
                        value: updateVal,
                        type: column.filter.type,
                        operator: 'lt'
                    }));
                } else {
                    var operator = (field.operator || column.filter.operator);

                    if (column.extraOperator) {
                        var decimalPrecision = (column.decimalPrecision || column.filter.decimalPrecision || 0);
                        var additionalDivision = (column.additionalDivision || 1);
                        var precision = Math.pow(10, -decimalPrecision) / 2;
                        //var minValue = (parseFloat(newVal) - precision) * additionalDivision
                        //var maxValue = (parseFloat(newVal) + precision) * additionalDivision
                        if (column.extraOperator == 'gte_lt') {
                            me.filterArray.push(Ext.create('Ext.util.Filter', {
                                property: column.dataIndex,
                                value: (parseFloat(newVal) - precision) * additionalDivision,
                                type: column.filter.type,
                                operator: 'gte'
                            }));

                            me.filterArray.push(Ext.create('Ext.util.Filter', {
                                property: column.dataIndex,
                                value: (parseFloat(newVal) + precision) * additionalDivision,
                                type: column.filter.type,
                                operator: 'lt'
                            }));
                        }
                    } else {
                        if (Ext.isArray(newVal)) {
                            if (newVal.length > 1) {
                                operator = 'in';
                            } else if (newVal.length == 1) {
                                newVal = newVal[0];
                            }
                        }

                        me.filterArray.push(Ext.create('Ext.util.Filter', {
                            property: column.dataIndex,
                            value: newVal,
                            type: column.filter.type,
                            operator: operator
                        }));
                    }
                }
            }
            if (!column.getEl().hasCls(me.columnFilteredCls)) {
                column.getEl().addCls(me.columnFilteredCls);
            }
        } else {
            if (column.getEl().hasCls(me.columnFilteredCls)) {
                column.getEl().removeCls(me.columnFilteredCls);
            }
        }
        //grid.store.currentPage = 1;
        if (me.filterArray.length > 0) {
            if (!grid.store.remoteFilter) grid.store.clearFilter();
            grid.store.filters.clear();
            grid.store.filter(me.filterArray);
            if (me.clearAllEl) {
                me.clearAllEl.show({ duration: 1000 });
            }
        } else {
            grid.store.clearFilter();
            if (me.clearAllEl) {
                me.clearAllEl.hide({ duration: 1000 });
            }
        }
        if (!grid.store.remoteFilter && me.autoUpdateAutoStores) {
            me.fillAutoStores();
        }
        me.grid.fireEvent('filterupdated', me.filterArray);
    },

    // private
    applyDelayedFilters: function (field) {
        if (!field.isValid()) return;
        var me = this;

        me.task.delay(me.updateBuffer, me.applyFilters, me, [field]);
    },

    // private
    applyInstantFilters: function (field) {
        var me = this;
        if (me.grid.fireEvent('beforefilterupdate', me) === false) return;
        if (!field.isValid()) return;
        if (field.isApplied()) return;
        //if (!field.isDirty()) return;
        field.setApplied(true);
        //field.resetOriginalValue();
        me.task.delay(0, me.applyFilters, me, [field]);
    },

    //private
    getFirstField: function () {
        var me = this,
            field = undefined;

        Ext.each(me.grid.query('gridcolumn'), function (col) {
            if (col.filter) {
                field = me.fields.get(col.dataIndex);
                return false;
            }
        });

        return field;
    },

    //private
    focusFirstField: function () {
        var me = this;

        var field = me.getFirstField();

        if (field && me.needFocus) {
            field.focus(false, 200);
        }
    },

    clearFilters: function (suppressEvent) {
        var me = this;

        //if (me.filterArray.length == 0) return;
        me.filterArray = [];
        me.fields.eachKey(function (key, field) {
            field.suspendEvents();
            field.setApplied(true);
            field.reset();
            field.resumeEvents();
            var column = me.columns.get(key);
            if (column.getEl().hasCls(Ext.baseCSSPrefix + 'column-filtered')) {
                column.getEl().removeCls(Ext.baseCSSPrefix + 'column-filtered');
            }
        }, me);
        me.grid.store.clearFilter(suppressEvent);
        if (me.clearAllEl) {
            me.clearAllEl.hide({ duration: 1000 });
        }

        me.grid.fireEvent('filterupdated', me.filterArray);
    },

    isVisible: function () {
        var me = this;

        return me.visible;
    },

    setVisible: function (visible) {
        var me = this;

        me.containers.each(function (item) {
            item.setVisible(visible);
        });

        if (visible) {
            me.focusFirstField();
        }
        me.grid.headerCt.doLayout();
        me.visible = visible;
    }
});
