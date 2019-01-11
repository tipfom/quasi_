﻿using System;
using System.Collections.Generic;
using Core.World.Components;
using Core.World.Serialization;

namespace Core.World {
    public class Entity {
        #region static 
        private static int currentInstance = 0;
        private static int currentSpecies = 0;
        private static Dictionary<int, string> entityNames = new Dictionary<int, string>( );
        #endregion

        public readonly int ID;
        public readonly int Species;
        public readonly IEntityWorld World;
        public EntityDomain Domain;
        public Transform Transform;
        public bool IsDestroyed;
        public event Action Destroyed;
        public bool IsOnScreen;
        public Vector2 PositionOnScreen;
        public Component[ ] Components;

        public string Name { get { return entityNames[Species]; } }

        private Queue<object[ ]>[ ] pendingComponentInfos = new Queue<object[ ]>[14];

        public Entity(ComponentList Components, Transform Transform, IEntityWorld World, int Species) {
            this.World = World;
            this.Transform = Transform;
            this.Species = Species;
            this.ID = ++currentInstance;

            for (int i = 0; i < pendingComponentInfos.Length; i++) {
                pendingComponentInfos[i] = new Queue<object[ ]>( );
            }

            this.Components = new Component[Components.Count];
            for (int i = 0; i < this.Components.Length; i++) {
                this.Components[i] = Components[i].Create(this);
            }

            World.Add(this);
        }

        ~Entity( ) {
            Destroy( );
        }

        public bool HasComponentInfo(ComponentData data) {
            return pendingComponentInfos[(int)data].Count > 0;
        }

        public object[ ] GetComponentInfo(ComponentData data) {
            // not containing needs to be handled with HasComponentInfo
            return pendingComponentInfos[(int)data].Dequeue( );
        }

        public void SetComponentInfo(ComponentData target, params object[ ] data) {
            pendingComponentInfos[(int)target].Enqueue(data);
        }

        public bool HasComponent<T>( ) {
            for (int i = 0; i < Components.Length; i++) {
                if (Components[i].GetType( ) == typeof(T)) {
                    return true;
                }
            }
            return false;
        }

        public T GetComponent<T>( ) where T : Component {
            for (int i = 0; i < Components.Length; i++) {
                if (Components[i].GetType( ) == typeof(T)) {
                    return (T)Components[i];
                }
            }
            return null;
        }

        public IEnumerable<Component> GetComponents( ) {
            for (int i = 0; i < Components.Length; i++)
                yield return Components[i];
        }

        public void Collision(Entity collidingEntity) {
            for (int i = 0; i < Components.Length; i++)
                Components[i].Collision(collidingEntity);
        }

        public void Destroy( ) {
            if (IsDestroyed)
                return;

            IsDestroyed = true;
            for (int i = 0; i < Components.Length; i++)
                Components[i].Destroy( );
            Destroyed?.Invoke( );
            World.Destroy(this);
        }

        public void Load(Dictionary<DataID,object> data) {
            for (int i = 0; i < Components.Length; i++)
                Components[i].Load(data);
        }

        public void Prepare( ) {
            for (int i = 0; i < Components.Length; i++)
                Components[i].Prepare( );
        }

        public void Tick( ) {
            for (int i = 0; i < Components.Length; i++)
                Components[i].Tick( );
        }

        public void Update(DeltaTime dt) {
            for (int i = 0; i < Components.Length; i++)
                Components[i].Update(dt);
        }

        public void Draw( ) {
            for (int i = 0; i < Components.Length; i++)
                Components[i].Draw( );
        }

        public override string ToString( ) {
            return Name;
        }

        public override int GetHashCode( ) {
            return ID;
        }

        public class Configuration {
            public ComponentList Components;
            public string Name;
            public Transform Transform;
            public int Species = -1;

            public Configuration( ) {
            }

            public Configuration(string name, Vector2 size) {
                Name = name;
                Transform = new Transform(default(Vector2), size);
                Components = new ComponentList( );
            }

            public Entity Create(Vector2 spawnLocation, IEntityWorld world, bool liftPosition = true) {
                if (Species == -1 || Components.HasChanged) {
                    Species = ++currentSpecies;
                    entityNames.Add(Species, Name);
                    Components.ResolveComponentDependencies( );
                    Components.Sort( );
                }
                if (liftPosition)
                    spawnLocation += new Vector2(0, Transform.HalfSize.Y);
                return new Entity(Components, new Transform(spawnLocation, Transform.Size), world, Species);
            }
        }
    }
}
