using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;
using FluentAssertions.Equivalency;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // в прошлой версии была проблема, что тестирование происходило отдельно по каждому полю
        // при добавлении новых полей или свойств в класс их нужно было бы дописать и в тесты
        // плюс можно случайно упустить какое-то поле
        // в этом и есть проблема 
        actualTsar.Should().BeEquivalentTo(expectedTsar, options => options.Excluding(
            (IMemberInfo member) => member.Name == nameof(Person.Id)));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 

        // все еще остается проблема, что мы можем упустить поле и что при добавлении новых полей
        // их так же нужно дописать, только уже в метод AreEqual, это уже проще
        // при этом в случае неуспешного теста будет мало информации, т.к.
        // ClassicAssert.True просто сообщит, что ожидалось True, а получено False
        // кроме того, из-за использования && в условии в случае, например, неверного имени
        // мы не узнаем результаты проверки возраста, роста ... т.е. остальных полей
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
