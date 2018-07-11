Better4You = Ember.Application.create({
    rootElement: '#emberBody',
});

Better4You.Store = DS.Store.extend({
    revision: 11,
    adapter: 'DS.FixtureAdapter'
});

Better4You.Router.map(function () {
    this.resource('items', function () {
        this.resource('detail', { path: 'item/:item_id' });
        this.resource('create', { path: "item/" });
    });
    //this.resource("items", { path: "/" });
    //this.resource("create", { path: "item" });
    //this.resource("detail", { path: "item/:item_id" });
});

Better4You.IndexRoute = Ember.Route.extend({
    
    redirect: function () {
        this.transitionTo('items');
    }
    
});

Better4You.ItemsRoute = Ember.Route.extend(
    {
        model: function() {
            return Better4You.Food.find();
        },
        events: {
            gotoDetail: function (model) {
                alert("detail");
                this.transitionTo('detail', model);
            },
            create: function (model) {
                alert("create");
                this.transitionTo('create');
            }
        }
    });

Better4You.ItemController = Ember.ObjectController.extend({
    content: null,
    foodTypes: [],
    ingredientTypes: [],
    fieldIds: [],
    fieldValues: {},
    isEditing: false,
    //gotoDetail: function (model) {
    //    alert("detail");
    //    this.transitionTo('detail', model);
    //},
    edit: function () {
        return this.set('isEditing', true);
    },
    doneEditing: function () {
        return this.set('isEditing', false);
    },
    create: function () {
        alert('create');
        this.set('foodTypes', Better4You.FoodType.find());
        this.set('fieldIds', Better4You.IngredientType.find());
        this.set('content', Better4You.Food.createRecord());
    },
    doneCreate: function (model) {
        //http://jsfiddle.net/PXnWj/
        //http://stackoverflow.com/questions/15162869/ember-textfield-valuebinding-with-dynamic-property
        //var foodModel = Better4YouItemManagment.Food.createRecord({name:model.get('name'),foodType:model.get('foodType')});
        var fieldValues = this.get('fieldValues');
        this.get('fieldIds').map(function (name) {
            if (fieldValues[name.id] != undefined) {
                Better4You.FoodIngredient.createRecord({ ingredientType: name, value: fieldValues[name.id], food: model });
                //var foodIngredient = Better4YouItemManagment.FoodIngredient.createRecord({ ingredientType: name, value: fieldValues[name.id], food: model });
                //model.get("foodIngredients").pushObject(foodIngredient);
            }
        });
        this.get('store').commit();
        this.set('fieldValues', {});
        this.transitionTo('index');
    }
});

Better4You.Food = DS.Model.extend({
    name: DS.attr('string'),
    foodType: DS.belongsTo('Better4You.FoodType'),
    foodIngredients: DS.hasMany('Better4You.FoodIngredient')
});

Better4You.Food.FIXTURES = [
    { id: 1, name: "food1", foodType: 106001, foodIngredients: [1, 2, 3, 4, 5] },
    { id: 2, name: "food2", foodType: 106002, foodIngredients: [6, 7, 8, 9, 10] },
    { id: 3, name: "food3", foodType: 106003, foodIngredients: [11, 12, 13, 14, 15] },
    { id: 4, name: "food4", foodType: 106004, foodIngredients: [16, 17, 18, 19, 20] }
];
Better4You.FoodIngredient = DS.Model.extend({
    description: DS.attr('string'),
    value: DS.attr('string'),
    food: DS.belongsTo('Better4You.Food'),
    ingredientType: DS.belongsTo('Better4You.IngredientType')
});

Better4You.FoodIngredient.FIXTURES = [
    { id: 1, description: "test food1", value: 1, food: 1, ingredientType: 109001 },
    { id: 2, description: "test food2", value: 1, food: 1, ingredientType: 109002 },
    { id: 3, description: "test food3", value: 1, food: 1, ingredientType: 109003 },
    { id: 4, description: "test food4", value: 1, food: 1, ingredientType: 109004 },
    { id: 5, description: "test food5", value: 1, food: 1, ingredientType: 109005 },
    { id: 6, description: "test food1", value: 1, food: 2, ingredientType: 109001 },
    { id: 7, description: "test food2", value: 1, food: 2, ingredientType: 109002 },
    { id: 8, description: "test food3", value: 1, food: 2, ingredientType: 109003 },
    { id: 9, description: "test food4", value: 1, food: 2, ingredientType: 109004 },
    { id: 10, description: "test food5", value: 1, food: 2, ingredientType: 109005 },
    { id: 11, description: "test food1", value: 1, food: 3, ingredientType: 109001 },
    { id: 12, description: "test food2", value: 1, food: 3, ingredientType: 109002 },
    { id: 13, description: "test food3", value: 1, food: 3, ingredientType: 109003 },
    { id: 14, description: "test food4", value: 1, food: 3, ingredientType: 109004 },
    { id: 15, description: "test food5", value: 1, food: 3, ingredientType: 109005 },
    { id: 16, description: "test food1", value: 1, food: 4, ingredientType: 109001 },
    { id: 17, description: "test food2", value: 1, food: 4, ingredientType: 109002 },
    { id: 18, description: "test food3", value: 1, food: 4, ingredientType: 109003 },
    { id: 19, description: "test food4", value: 1, food: 4, ingredientType: 109004 },
    { id: 20, description: "test food5", value: 1, food: 4, ingredientType: 109005 }
];

Better4You.FoodType = DS.Model.extend({
    name: DS.attr('string'),
});

Better4You.FoodType.FIXTURES = [
{ id: 106001, name: 'Milk' },
{ id: 106002, name: 'Fruit' },
{ id: 106003, name: 'Grain' },
{ id: 106004, name: 'Meat Alternative' }
];

Better4You.IngredientType = DS.Model.extend({
    name: DS.attr('string'),
});

Better4You.IngredientType.FIXTURES = [
{ id: 109001, name: 'Portion Size' },
{ id: 109002, name: 'Calories' },
{ id: 109003, name: 'Cholesterol' },
{ id: 109004, name: 'Sodium' },
{ id: 109005, name: 'Fiber' },
{ id: 109006, name: 'Iron' },
{ id: 109007, name: 'Calcium' },
{ id: 109008, name: 'Vitamin A' },
{ id: 109009, name: 'Vitamin B' },
{ id: 109010, name: 'Vitamin C' },
{ id: 109011, name: 'Protein' },
{ id: 109012, name: 'Carbs' },
{ id: 109013, name: 'Total Fat' },
{ id: 109014, name: 'Sat Fat' },
{ id: 109015, name: 'Trans Fat' }
];


Better4You.TextField = Ember.TextField.extend({
    fieldId: null,
    values: null,

    updateValues: function () {
        var fieldId = this.get('fieldId');
        var values = this.get('values');
        if (values && fieldId) values[fieldId.id] = this.get('value');
    }.observes('value')
});
/*
Ember.Handlebars.registerBoundHelper('date', function (date) {
    return moment(date).fromNow();
});

var showdown = new Showdown.converter();

Ember.Handlebars.registerBoundHelper('markdown', function (input) {
    //return input;
    return new Ember.Handlebars.SafeString(showdown.makeHtml(input));
});
*/