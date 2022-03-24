namespace BOG
{
    //A class can prevent other classes from inheriting from it, or from any of its members, by declaring itself or the member as sealed.
    public sealed class NormalMeshGenerator : MeshGenerator
    {
        protected override void OnEnable()
        {
            //GREAT EXAMPLE of combination of inheritance, polymorphism & generics
            GenerateMesh<NormalMeshHandler>();
            base.OnEnable();
        }
    }
}
