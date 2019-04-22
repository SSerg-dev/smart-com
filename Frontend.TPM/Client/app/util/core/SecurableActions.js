Ext.define('App.util.core.SecurableActions', {

    constructor: function () {

    },

    afterRender: function () {
        this.getButtons().forEach(function (button) {
            this.setVisibility(button);
        }, this);
    },

    setVisibility: function (cmp) {
        var items = cmp.query('> menuitem'),
            isVisible;

        if (!Ext.isEmpty(items)) {
            isVisible = items.map(function (item) {
                return this.setVisibility(item);
            }, this).reduce(function (result, item) {
                return item || result;
            }, false);
            //console.trace('Node', cmp.text, isVisible);
        } else {
            isVisible = this.checkVisibility(cmp);
            //console.trace('Leaf', cmp.text, isVisible);
        }

        cmp.setVisible(isVisible);

        return isVisible;
    },

    checkVisibility: function (cmp) {
        var hasValidActionProperty = cmp.hasOwnProperty('action') && Ext.isString(cmp.action) && !Ext.isEmpty(cmp.action);

        if (!hasValidActionProperty) {
            return true;
        }

        // Дополнительная логика по скрытию кнопок
        // Кнопка не отобразится на гридах из списка directoryExclusionList (если он задан)
        // А так же кнопка отобразится только в грида из списка directoryList (если он задан)
        // Сравнение происходит по alias виджета справочника
        var directoryList = cmp.directoryList;
        var exeptDirectories = cmp.exeptDirectories;

        if (directoryList && directoryList.length) {
            var panelInList = Ext.Array.contains(directoryList, this.xtype);
            if ((!panelInList && !exeptDirectories) || (panelInList && exeptDirectories)) {
                return false;
            }
        }

        var defaultResource = this.getDefaultResource(cmp.exactlyModelCompare) || '',
            resource, action;

        resource = Ext.String.format(cmp.resource || defaultResource, defaultResource);

        if (Ext.isEmpty(resource)) {
            console.warn('Resource does not set for', cmp.text, 'in', this.text || Ext.getClassName(this));
            return false;
        }

        action = Ext.String.format(cmp.action, resource);

        return App.UserInfo.hasAccessPoint(resource, action);
    },

    //getDefaultResourceName: function (button) {
    //    var result = null;
    //    var panel = button.up('combineddirectorypanel');
    //    if (panel) {
    //        var grid = panel.down('directorygrid');
    //        if (grid) {
    //            var proxy = grid.getStore().getProxy();
    //            result = proxy.resourceName;
    //        }
    //    }
    //    return result;
    //},//Ext.emptyFn,

    getButtons: function () {
        return this.query('button[action],button[menu]');
    }

});