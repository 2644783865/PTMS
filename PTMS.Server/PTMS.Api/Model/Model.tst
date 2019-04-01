${
    // Enable extension methods by adding using Typewriter.Extensions.*
    using Typewriter.Extensions.Types;

    // Uncomment the constructor to change template settings.
    Template(Settings settings)
    {
        settings.OutputFilenameFactory = file => {
            var name = file.Classes.First().Name;

            name = name.Replace("Model", "Dto");

            return name;
        };
    }

    // Custom extension methods can be used in the template by adding a $ prefix e.g. $LoudName
    string LoudName(Property property)
    {
        return property.Name.Replace("Model", "Dto");
    }
}

$Classes(*Model)[
    export interface $Name {
        $Properties[
        $name: $Type]
    }]