Ext.define('App.view.tpm.mechanictype.BaseClientTreeSearchField', {
    extend: 'App.view.tpm.nonenego.TreeSearchField',
    alias: 'widget.baseclienttreesearchfield',
    mixins: ['Ext.util.Bindable'],

    trigger1Cls: Ext.baseCSSPrefix + 'form-search-trigger',
    trigger2Cls: Ext.baseCSSPrefix + 'form-clear-trigger',

    onTrigger1Click: function () {
        var picker = this.createPicker();

        if (picker) {
            picker.show();
            var selectorWidget = picker.down(this.selectorWidget),
                elementsToHide = selectorWidget.query('[hierarchyOnly=true]');
            elementsToHide.forEach(function (el) { el.hide(); });
        }
    },

    onTrigger2Click: function () {
        this.clearValue();
    },

    onSelectionChange: function (selModel) {
        var picker = this.picker,
            hasSelection = selModel.hasSelection();

        if (picker && hasSelection) {
            var record = selModel.selected.items[0].data;
            picker.down('#select')[record.IsBaseClient ? 'enable' : 'disable']();
        }
    }
});