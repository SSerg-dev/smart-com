Ext.define('App.controller.tpm.filter.Filter', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'filterconstructor #addNode': {
                    click: this.onAddNodeButtonClick
                },
                'filterconstructor #addRule': {
                    click: this.onAddRuleButtonClick
                },
                'filterconstructor #delete': {
                    click: this.onDeleteButtonClick
                },
                'filterconstructor #operationButton': {
                    click: this.onOperationButtonClick
                },

                'filterconstructor combobox[name=filterfield]': {
                    change: this.onFilterFieldChange
                },
                'filterconstructor combobox[name=operationfield]': {
                    change: this.onOperationFieldChange
                },

                'filterconstructor #apply': {
                    click: this.onApplyFilterButtonClick
                },
                'filterconstructor #reset': {
                    click: this.onResetFilterButtonClick
                },
              
                'selectoperationwindow #addpanel': {
                    click: this.onAddPanelButtonClick
                }
            }
        })
    },

    onAddNodeButtonClick: function (button) {
        var selectoperationwindow = Ext.widget({
            xtype: 'selectoperationwindow',
            content: button.up('filterconstructornode').down('container[name=content]')
        });
        selectoperationwindow.show();
    },
    
    onAddRuleButtonClick: function (button) {
        var filterconstructor = button.up('filterconstructor'),
            content = button.up('filterconstructornode').down('container[name=rulecontent]'),
            rule = Ext.create('App.view.tpm.filter.FilterConstructiorRule'),
            fields = rule.down('combobox[name=filterfield]');
        filterconstructor.store.clearFilter();
        fields.store = filterconstructor.store;

        rule.setMargin('0 0 0 60');
        content.add(rule);
    },

    onAddPanelButtonClick: function (button) {
        var window = button.up('selectoperationwindow'),
            radiogroup = window.down('radiogroup'),
            checked = radiogroup.getChecked(),
            node = Ext.create('App.view.tpm.filter.FilterConstructiorNode'),
            content;

        if (window.content) {
            content = window.content;
            node.down('#delete').setDisabled(false);
        } else {
            //выбор самого первого узла фильтра (при открытии окна)
            var filterWindow = Ext.widget('filterproduct');
            content = filterWindow.down('filterconstructor').down('container[name=filtercontainer]');
            window.first = false;
            filterWindow.show();
        }

        //OR или AND
        if (checked[0].inputValue === 1) {
            node.addCls('andnodefilterpanel');
            node.down('#operationButton').setText('AND');
        } else if (checked[0].inputValue === 2) {
            node.addCls('ornodefilterpanel');
            node.down('#operationButton').setText('OR');
        };

        content.add(node);
        window.close();
    },

    onOperationButtonClick: function (button) {
        var node = button.up('filterconstructornode');
        if (node.hasCls('andnodefilterpanel')) {
            node.removeCls('andnodefilterpanel');
            node.addCls('ornodefilterpanel');
            button.setText('OR');
        } else if (node.hasCls('ornodefilterpanel')) {
            node.removeCls('ornodefilterpanel');
            node.addCls('andnodefilterpanel');
            button.setText('AND');
        }
    },

    onDeleteButtonClick: function (button) {
        var constructor = button.up('filterconstructorrule');
        if (!constructor)
            constructor = button.up('filterconstructornode');
        constructor.destroy();
    },

    onFilterFieldChange: function (field, newValue, oldValue) {
        var operationField = field.up().down('combobox[name=operationfield]');
        operationField.getStore().loadData(newValue.getAllowedOperations().map(function (op) {
            return {
                id: op,
                text: l10n.ns('core', 'filter', 'operations').value(op)
            };
        }));
    },

    onOperationFieldChange: function (field, newValue, oldValue) {
        var valueContainer = field.up().down('container[name=valuecontainer]'),
            filterField = field.up().down('combobox[name=filterfield]'),
            filterValue = filterField.getValue();

        var currentValueCmp = valueContainer.child(),
            editorFactory = App.view.core.filter.ValueEditorFactory;

        filterValue.set('operation', newValue);
        filterValue.set('value', null);
        var valueCmp = editorFactory.createEditor(filterValue, currentValueCmp);

        if (valueCmp && valueCmp !== currentValueCmp) {
            valueCmp.setMargin(1);
            valueCmp.setHeight(22);
            valueCmp.addCls('filterText');
        }

        if (valueCmp !== currentValueCmp) {
            Ext.suspendLayouts();
            valueContainer.removeAll();
            valueContainer.add(valueCmp);
            Ext.resumeLayouts(true);
        } 
    },

    onApplyFilterButtonClick: function (button) {
        var filterconstructor = button.up('filterconstructor'),
            filtercontainer = filterconstructor.down('container[name=filtercontainer]'),
            productGrid = filterconstructor.up().down('product').down('directorygrid'),
            productStore = productGrid.getStore(),
            extendedFilter = productStore.extendedFilter,
            filter = this.parseConstructorFilter(filtercontainer),
            customTextFilterModel = Ext.create('App.model.tpm.filter.CustomTextFilterModel'),
            notStringFilter = this.getStringFilter(filtercontainer),
            stringFilter = JSON.stringify(customTextFilterModel.serializeFilter(notStringFilter), '', 4),
            window = filterconstructor.up('filterproduct');     

        if (filter) {         
            if (window.expandProductPanel === undefined || window.expandProductPanel) {
                filterconstructor.toggleCollapse();
                if (filterconstructor.up().down('product').collapsed) {
                    filterconstructor.up().down('product').expand();
                }
            } else {
                window.expandProductPanel = true;
            }

            window.stringFilter = stringFilter;
            extendedFilter.filter = filter;
            extendedFilter.reloadStore();
        } else {
            App.Notify.pushInfo('Filter is empty');
        }
    },

    onResetFilterButtonClick: function (button) {
        var filterconstructor = button.up('filterconstructor'),
           productGrid = filterconstructor.up().down('product').down('directorygrid'),
           productStore = productGrid.getStore(),
           extendedFilter = productStore.extendedFilter,
           content = button.up('filterconstructor').down('container[name=filtercontainer]');

        // очистка контейнера фильтра
        content.removeAll();
        var selectoperationwindow = Ext.widget({
            xtype: 'selectoperationwindow',
            content: content
        });
        selectoperationwindow.show();

        // очистка фильтра стора продуктов в гриде 
        extendedFilter.clear();
    },

    parseConstructorFilter: function (container) {
        if (!container)
            return null;
        var node = container.down('filterconstructornode');
        //TODO: сделать валидацию узла!
        //if (!filterHelper.isValidNode(node))
        //    return null;
        var jsonObj = {};
        jsonObj.rules = new Array();
        var operation = container.down('#operationButton');
        if (!operation)
            return null;
        jsonObj.operator = operation.getText().toLowerCase();
        var rulecontent = container.down('container[name=rulecontent]');
        for (var i = 0; i < rulecontent.items.items.length; i++) {
            var item = rulecontent.items.items[i],
                operation = item.down('combobox[name=operationfield]').getValue(),
                valueField = item.down('container[name=valuecontainer]').down('field');
            if (valueField)
                var val = item.down('container[name=valuecontainer]').down('field').getValue();
            if (operation === 'In') {
                var jsonInObj = {};
                jsonInObj.rules = new Array();
                jsonInObj.operator = "or";
                for (var j = 0; j < val.values.length; j++) {
                    var filterField = item.down('combobox[name=filterfield]');
                    var property = filterField.getValue() ? filterField.getValue().data.id : null;
                    if (property) {
                        var rule = {
                            operation: "Equals",
                            property: property,
                            value: val.values[j],
                        };
                        jsonInObj.rules.push(rule);
                    } else {
                        item.hide();
                    }
                }
                jsonObj.rules.push(jsonInObj);
            } else if (operation === 'IsNull') {
                var jsonIsNullObj = {};
                jsonIsNullObj.rules = new Array();
                jsonIsNullObj.operator = "or";
                var filterField = item.down('combobox[name=filterfield]');
                var property = filterField.getValue() ? filterField.getValue().data.id : null;
                if (property) {
                    var rule = {
                        operation: "Equals",
                        property: property,
                        value: null,
                    };
                    jsonIsNullObj.rules.push(rule);
                    rule = {
                        operation: "Equals",
                        property: property,
                        value: '',
                    };
                    jsonIsNullObj.rules.push(rule);
                    jsonObj.rules.push(jsonIsNullObj);
                } else {
                    item.hide();
                }
            } else if (operation === 'NotNull') {
                var jsonNotNullObj = {};
                jsonNotNullObj.rules = new Array();
                jsonNotNullObj.operator = "and";
                var filterField = item.down('combobox[name=filterfield]');
                var property = filterField.getValue() ? filterField.getValue().data.id : null;
                if (property) {
                    var rule = {
                        operation: "NotEqual",
                        property: property,
                        value: null,
                    };
                    jsonNotNullObj.rules.push(rule);
                    rule = {
                        operation: "NotEqual",
                        property: property,
                        value: '',
                    };
                    jsonNotNullObj.rules.push(rule);
                    jsonObj.rules.push(jsonNotNullObj);
                }
            } else {
                var filterField = item.down('combobox[name=filterfield]');
                var property = filterField.getValue() ? filterField.getValue().data.id : null;
                if (property) {
                    var rule = {
                        operation: operation,
                        property: property,
                        value: val ? val : '',
                    };
                    jsonObj.rules.push(rule);
                } else {
                    item.hide();
                }
            }
        }
        var nodecontent = container.down('container[name=content]');
        for (var i = 0; i < nodecontent.items.items.length; i++) {
            var item = nodecontent.items.items[i];
            jsonObj.rules.push(this.parseConstructorFilter(item));
        }
        if (jsonObj.rules.length == 0)
            return null;
        return jsonObj;
    },

    getStringFilter: function (container) {
        if (!container)
            return null;
        var node = container.down('filterconstructornode');
        //TODO: сделать валидацию узла!
        //if (!filterHelper.isValidNode(node))
        //    return null;
        var jsonObj = {};
        jsonObj.rules = new Array();
        var operation = container.down('#operationButton');
        if (!operation)
            return null;
        jsonObj.operator = operation.getText().toLowerCase();
        var rulecontent = container.down('container[name=rulecontent]');
        for (var i = 0; i < rulecontent.items.items.length; i++) {
            var item = rulecontent.items.items[i],
                operation = item.down('combobox[name=operationfield]').getValue(),
                valueField = item.down('container[name=valuecontainer]').down('field');
            if (valueField)
                var val = item.down('container[name=valuecontainer]').down('field').getValue();

            var filterField = item.down('combobox[name=filterfield]');
            var property = filterField.getValue() ? filterField.getValue().data.id : null;
            if (property) {
                var rule = {
                    operation: operation,
                    property: property,
                    value: val,
                };
                jsonObj.rules.push(rule);
            } else {
                item.hide();
            }
        }
        var nodecontent = container.down('container[name=content]');
        for (var i = 0; i < nodecontent.items.items.length; i++) {
            var item = nodecontent.items.items[i];
            jsonObj.rules.push(this.getStringFilter(item));
        }
        if (jsonObj.rules.length == 0)
            return null;
        return jsonObj;
    }
})