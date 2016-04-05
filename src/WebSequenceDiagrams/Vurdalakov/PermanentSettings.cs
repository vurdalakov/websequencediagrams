namespace Vurdalakov
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Reflection;

    public class PermanentSettings
    {
        private readonly String _fileName;
        private readonly Dictionary<String, Object> _settings = new Dictionary<String, Object>();

        public PermanentSettings() : this("PermanentSettings.txt")
        {
        }

        public PermanentSettings(String fileName)
        {
            _fileName = fileName;

            using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                using (var streamReader = new StreamReader(new IsolatedStorageFileStream(_fileName, FileMode.OpenOrCreate, isolatedStorageFile)))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var parts = streamReader.ReadLine().Split('=');
                        if (parts.Length != 3)
                        {
                            continue;
                        }

                        var type = Type.GetType(parts[1]);
                        if (null == type)
                        {
                            type = Assembly.GetExecutingAssembly().GetType(parts[1]);
                            if (null == type)
                            {
                                continue;
                            }
                        }

                        try
                        {
                            _settings.Add(parts[0], Convert.ChangeType(parts[2], type)); // normal type
                        }
                        catch
                        {
                            try
                            {
                                _settings.Add(parts[0], Enum.Parse(type, parts[2])); // enums
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }

        public void Save()
        {
            using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                using (var streamWriter = new StreamWriter(new IsolatedStorageFileStream(_fileName, FileMode.OpenOrCreate, isolatedStorageFile)))
                {
                    foreach (var setting in _settings)
                    {
                        streamWriter.WriteLine("{0}={1}={2}", setting.Key, setting.Value.GetType().ToString(), setting.Value);
                    }
                }
            }
        }

        public Boolean Contains(String name)
        {
            return _settings.ContainsKey(name);
        }

        public Object Get(String name)
        {
            return _settings.ContainsKey(name) ? _settings[name] : null;
        }

        public void Set(String name, Object value)
        {
            if (_settings.ContainsKey(name))
            {
                _settings[name] = value;
            }
            else
            {
                _settings.Add(name, value);
            }

            Save();
        }

        public void Remove(String name)
        {
            _settings.Remove(name);
        }

        public Object Get(String name, Object defaultValue)
        {
            return this.Contains(name) ? this.Get(name) : defaultValue;
        }

        public String Get(String name, String defaultValue)
        {
            return this.Get(name, (Object)defaultValue) as String;
        }

        public Boolean Get(String name, Boolean defaultValue)
        {
            return (Boolean)this.Get(name, (Object)defaultValue);
        }

        public Int32 Get(String name, Int32 defaultValue)
        {
            return (Int32)this.Get(name, (Object)defaultValue);
        }

        public Double Get(String name, Double defaultValue)
        {
            return (Double)this.Get(name, (Object)defaultValue);
        }

        public void CopyFrom(PermanentSettings permanentSettings, String name)
        {
            this.Set(name, permanentSettings.Get(name));
        }
    }
}
