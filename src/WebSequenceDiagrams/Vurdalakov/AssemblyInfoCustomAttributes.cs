namespace System.Reflection
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class AssemblyRepositoryUrl : Attribute
    {
        public AssemblyRepositoryUrl(String repositoryUrl)
        {
            this.RepositoryUrl = repositoryUrl;
        }

        public String RepositoryUrl { get; private set; }
    }
}
